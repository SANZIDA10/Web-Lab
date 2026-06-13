using System;

namespace WebLab.WebForms.Models
{
    public sealed class Submission
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Type { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string StudyProgram { get; set; }

        public string Interests { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTimeOffset SubmittedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}