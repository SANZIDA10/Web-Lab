using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebLab.Api.Data;
using WebLab.Api.Models;

// Get the Web Lab directory (parent of backend)
var siteRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".."));
var databasePath = Path.Combine(siteRoot, "backend", "data", "weblab.db");

Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = siteRoot,
    WebRootPath = siteRoot
});

builder.Services.AddDbContext<WebLabDbContext>(options =>
    options.UseSqlite($"Data Source={databasePath}"));

var app = builder.Build();
var fileProvider = new PhysicalFileProvider(siteRoot);

var defaultFiles = new DefaultFilesOptions
{
    FileProvider = fileProvider
};
defaultFiles.DefaultFileNames.Clear();
defaultFiles.DefaultFileNames.Add("home.html");

app.UseDefaultFiles(defaultFiles);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WebLabDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/api/health", () => Results.Ok(new
{
    status = "ok",
    service = "KUET Career Club backend"
}));

app.MapPost("/api/contact", async (ContactSubmission request, WebLabDbContext db, CancellationToken cancellationToken) =>
{
    var validationError = ValidateCommonSubmission(request.Name, request.Email, request.Message);
    if (validationError is not null)
    {
        return Results.BadRequest(new { message = validationError });
    }

    var submission = await CreateSubmissionAsync(db, new Submission
    {
        Type = "contact",
        Name = request.Name!.Trim(),
        Email = request.Email!.Trim(),
        Message = request.Message!.Trim(),
        StudyProgram = null,
        Interests = null
    }, cancellationToken);

    return Results.Ok(new
    {
        message = "Thanks! Your message has been received.",
        submissionId = submission.Id
    });
});

app.MapPost("/api/join", async (JoinSubmission request, WebLabDbContext db, CancellationToken cancellationToken) =>
{
    var validationError = ValidateCommonSubmission(request.Name, request.Email, request.Message);
    if (validationError is not null)
    {
        return Results.BadRequest(new { message = validationError });
    }

    var submission = await CreateSubmissionAsync(db, new Submission
    {
        Type = "join",
        Name = request.Name!.Trim(),
        Email = request.Email!.Trim(),
        StudyProgram = request.StudyProgram?.Trim(),
        Interests = request.Interests?.Trim(),
        Message = request.Message!.Trim()
    }, cancellationToken);

    return Results.Ok(new
    {
        message = "Your membership interest has been recorded.",
        submissionId = submission.Id
    });
});

app.MapGet("/api/submissions", async (WebLabDbContext db, CancellationToken cancellationToken) =>
{
    var submissions = await db.Submissions
        .AsNoTracking()
        .OrderByDescending(submission => submission.SubmittedAt)
        .ToListAsync(cancellationToken);

    return Results.Ok(submissions.Select(ToSubmissionResponse));
});

app.MapGet("/api/submissions/{id:guid}", async (Guid id, WebLabDbContext db, CancellationToken cancellationToken) =>
{
    var submission = await db.Submissions
        .AsNoTracking()
        .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

    return submission is null
        ? Results.NotFound(new { message = "Submission not found." })
        : Results.Ok(ToSubmissionResponse(submission));
});

app.MapPost("/api/submissions", async (Submission request, WebLabDbContext db, CancellationToken cancellationToken) =>
{
    var validationError = ValidateCommonSubmission(request.Name, request.Email, request.Message);
    if (validationError is not null)
    {
        return Results.BadRequest(new { message = validationError });
    }

    if (string.IsNullOrWhiteSpace(request.Type))
    {
        return Results.BadRequest(new { message = "Please provide a submission type." });
    }

    var submission = await CreateSubmissionAsync(db, new Submission
    {
        Type = request.Type!.Trim(),
        Name = request.Name!.Trim(),
        Email = request.Email!.Trim(),
        StudyProgram = request.StudyProgram?.Trim(),
        Interests = request.Interests?.Trim(),
        Message = request.Message!.Trim()
    }, cancellationToken);

    return Results.Created($"/api/submissions/{submission.Id}", ToSubmissionResponse(submission));
});

app.MapPut("/api/submissions/{id:guid}", async (Guid id, Submission request, WebLabDbContext db, CancellationToken cancellationToken) =>
{
    var validationError = ValidateCommonSubmission(request.Name, request.Email, request.Message);
    if (validationError is not null)
    {
        return Results.BadRequest(new { message = validationError });
    }

    if (string.IsNullOrWhiteSpace(request.Type))
    {
        return Results.BadRequest(new { message = "Please provide a submission type." });
    }

    var submission = await db.Submissions.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
    if (submission is null)
    {
        return Results.NotFound(new { message = "Submission not found." });
    }

    submission.Type = request.Type.Trim().ToLowerInvariant();
    submission.Name = request.Name!.Trim();
    submission.Email = request.Email!.Trim();
    submission.StudyProgram = request.StudyProgram?.Trim();
    submission.Interests = request.Interests?.Trim();
    submission.Message = request.Message!.Trim();

    await db.SaveChangesAsync(cancellationToken);

    return Results.Ok(ToSubmissionResponse(submission));
});

app.MapDelete("/api/submissions/{id:guid}", async (Guid id, WebLabDbContext db, CancellationToken cancellationToken) =>
{
    var submission = await db.Submissions.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
    if (submission is null)
    {
        return Results.NotFound(new { message = "Submission not found." });
    }

    db.Submissions.Remove(submission);
    await db.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});

app.MapGet("/api/members", async (WebLabDbContext db, CancellationToken cancellationToken) =>
{
    var members = await LoadMembersAsync(db, cancellationToken);
    return Results.Ok(new
    {
        count = members.Count,
        members = members
    });
});

app.MapGet("/api/members/search", async (string? q, WebLabDbContext db, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(q))
    {
        var members = await LoadMembersAsync(db, cancellationToken);
        return Results.Ok(members);
    }

    var searchTerm = q.Trim().ToLowerInvariant();
    var results = await db.Submissions
        .AsNoTracking()
        .Where(submission => submission.Type == "join")
        .Where(submission =>
            (submission.Name ?? string.Empty).ToLower().Contains(searchTerm) ||
            (submission.Email ?? string.Empty).ToLower().Contains(searchTerm) ||
            (submission.StudyProgram ?? string.Empty).ToLower().Contains(searchTerm))
        .OrderByDescending(submission => submission.SubmittedAt)
        .ToListAsync(cancellationToken);

    return Results.Ok(new
    {
        count = results.Count,
        searchTerm = q,
        members = results.Select(ToSubmissionResponse)
    });
});

app.MapGet("/api/members/stats", async (WebLabDbContext db, CancellationToken cancellationToken) =>
{
    var members = await LoadMembersAsync(db, cancellationToken);
    var programStats = members
        .Where(member => member.Data.ContainsKey("studyProgram") && !string.IsNullOrWhiteSpace(member.Data["studyProgram"]))
        .GroupBy(member => member.Data["studyProgram"])
        .ToDictionary(group => group.Key, group => group.Count());

    return Results.Ok(new
    {
        totalMembers = members.Count,
        joinedThisMonth = members.Count(member => member.SubmittedAt.Month == DateTimeOffset.UtcNow.Month),
        programDistribution = programStats
    });
});

app.Run();

static async Task<Submission> CreateSubmissionAsync(WebLabDbContext db, Submission submission, CancellationToken cancellationToken)
{
    submission.Id = submission.Id == Guid.Empty ? Guid.NewGuid() : submission.Id;
    submission.Type = submission.Type.Trim().ToLowerInvariant();
    submission.SubmittedAt = DateTimeOffset.UtcNow;

    db.Submissions.Add(submission);
    await db.SaveChangesAsync(cancellationToken);
    return submission;
}

static async Task<List<SubmissionResponse>> LoadMembersAsync(WebLabDbContext db, CancellationToken cancellationToken)
{
    var members = await db.Submissions
        .AsNoTracking()
        .Where(submission => submission.Type == "join")
        .OrderByDescending(submission => submission.SubmittedAt)
        .ToListAsync(cancellationToken);

    return members.Select(ToSubmissionResponse).ToList();
}

static SubmissionResponse ToSubmissionResponse(Submission submission)
{
    var data = new Dictionary<string, string>
    {
        ["name"] = submission.Name,
        ["email"] = submission.Email,
        ["message"] = submission.Message
    };

    if (!string.IsNullOrWhiteSpace(submission.StudyProgram))
    {
        data["studyProgram"] = submission.StudyProgram;
    }

    if (!string.IsNullOrWhiteSpace(submission.Interests))
    {
        data["interests"] = submission.Interests;
    }

    return new SubmissionResponse(submission.Id, submission.Type, submission.SubmittedAt, data);
}

static string? ValidateCommonSubmission(string? name, string? email, string? message)
{
    if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 2)
    {
        return "Please enter your full name.";
    }

    if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email.Trim()))
    {
        return "Please enter a valid email address.";
    }

    if (string.IsNullOrWhiteSpace(message) || message.Trim().Length < 10)
    {
        return "Your message should be at least 10 characters long.";
    }

    return null;
}

public sealed record ContactSubmission(string? Name, string? Email, string? Message);

public sealed record JoinSubmission(string? Name, string? Email, string? StudyProgram, string? Interests, string? Message);

public sealed record SubmissionResponse(Guid Id, string Type, DateTimeOffset SubmittedAt, IReadOnlyDictionary<string, string> Data);