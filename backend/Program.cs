using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.Extensions.FileProviders;

// Get the Web Lab directory (parent of backend)
var siteRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".."));

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = siteRoot,
    WebRootPath = siteRoot
});

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

// Load submissions from file on startup
var store = app.Services.GetRequiredService<SubmissionStore>();
store.LoadFromFile();

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

app.MapGet("/api/members", (SubmissionStore store) =>
{
    var members = store.GetMembers();
    return Results.Ok(new
    {
        count = members.Count,
        members = members
    });
});

app.MapGet("/api/members/search", (string? q, SubmissionStore store) =>
{
    if (string.IsNullOrWhiteSpace(q))
    {
        return Results.Ok(store.GetMembers());
    }

    var searchTerm = q.ToLowerInvariant();
    var results = store.GetMembers()
        .Where(m => 
            m.Data.ContainsKey("name") && m.Data["name"].Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            m.Data.ContainsKey("email") && m.Data["email"].Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            m.Data.ContainsKey("studyProgram") && m.Data["studyProgram"].Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
        )
        .ToList();

    return Results.Ok(new
    {
        count = results.Count,
        searchTerm = q,
        members = results
    });
});

app.MapGet("/api/members/stats", (SubmissionStore store) =>
{
    var members = store.GetMembers();
    var programStats = members
        .Where(m => m.Data.ContainsKey("studyProgram") && !string.IsNullOrWhiteSpace(m.Data["studyProgram"]))
        .GroupBy(m => m.Data["studyProgram"])
        .ToDictionary(g => g.Key, g => g.Count());

    return Results.Ok(new
    {
        totalMembers = members.Count,
        joinedThisMonth = members.Count(m => m.SubmittedAt.Month == DateTimeOffset.UtcNow.Month),
        programDistribution = programStats
    });
});

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
    private readonly string dataFilePath;
    private readonly object fileLock = new();

    public SubmissionStore()
    {
        var dataDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "data");
        Directory.CreateDirectory(dataDir);
        dataFilePath = Path.Combine(dataDir, "submissions.json");
    }

    public SubmissionRecord Add(string type, IReadOnlyDictionary<string, string> data)
    {
        var submission = new SubmissionRecord(Guid.NewGuid(), type, DateTimeOffset.UtcNow, data);
        submissions.Enqueue(submission);
        SaveToFile();
        return submission;
    }

    public IReadOnlyCollection<SubmissionRecord> GetAll()
    {
        return submissions.ToArray();
    }

    public List<SubmissionRecord> GetMembers()
    {
        return submissions.Where(s => s.Type == "join").ToList();
    }

    public void SaveToFile()
    {
        lock (fileLock)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(submissions.ToArray(), options);
                File.WriteAllText(dataFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving submissions: {ex.Message}");
            }
        }
    }

    public void LoadFromFile()
    {
        lock (fileLock)
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    var json = File.ReadAllText(dataFilePath);
                    var records = JsonSerializer.Deserialize<SubmissionRecord[]>(json);
                    if (records != null)
                    {
                        foreach (var record in records)
                        {
                            submissions.Enqueue(record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading submissions: {ex.Message}");
            }
        }
    }
}