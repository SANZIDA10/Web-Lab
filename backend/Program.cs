using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var siteRoot = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, ".."));

builder.WebHost.UseWebRoot(siteRoot);
builder.Services.AddSingleton<SubmissionStore>();

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

app.MapGet("/api/health", () => Results.Ok(new
{
    status = "ok",
    service = "KUET Career Club backend"
}));

app.MapPost("/api/contact", (ContactSubmission request, SubmissionStore store) =>
{
    var validationError = ValidateCommonSubmission(request.Name, request.Email, request.Message);
    if (validationError is not null)
    {
        return Results.BadRequest(new { message = validationError });
    }

    var submission = store.Add("contact", new Dictionary<string, string>
    {
        ["name"] = request.Name!.Trim(),
        ["email"] = request.Email!.Trim(),
        ["message"] = request.Message!.Trim()
    });

    return Results.Ok(new
    {
        message = "Thanks! Your message has been received.",
        submissionId = submission.Id
    });
});

app.MapPost("/api/join", (JoinSubmission request, SubmissionStore store) =>
{
    var validationError = ValidateCommonSubmission(request.Name, request.Email, request.Message);
    if (validationError is not null)
    {
        return Results.BadRequest(new { message = validationError });
    }

    var submission = store.Add("join", new Dictionary<string, string>
    {
        ["name"] = request.Name!.Trim(),
        ["email"] = request.Email!.Trim(),
        ["studyProgram"] = request.StudyProgram?.Trim() ?? string.Empty,
        ["interests"] = request.Interests?.Trim() ?? string.Empty,
        ["message"] = request.Message!.Trim()
    });

    return Results.Ok(new
    {
        message = "Your membership interest has been recorded.",
        submissionId = submission.Id
    });
});

app.MapGet("/api/submissions", (SubmissionStore store) => Results.Ok(store.GetAll()));

app.Run();

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

public sealed record SubmissionRecord(Guid Id, string Type, DateTimeOffset SubmittedAt, IReadOnlyDictionary<string, string> Data);

public sealed class SubmissionStore
{
    private readonly ConcurrentQueue<SubmissionRecord> submissions = new();

    public SubmissionRecord Add(string type, IReadOnlyDictionary<string, string> data)
    {
        var submission = new SubmissionRecord(Guid.NewGuid(), type, DateTimeOffset.UtcNow, data);
        submissions.Enqueue(submission);
        return submission;
    }

    public IReadOnlyCollection<SubmissionRecord> GetAll()
    {
        return submissions.ToArray();
    }
}