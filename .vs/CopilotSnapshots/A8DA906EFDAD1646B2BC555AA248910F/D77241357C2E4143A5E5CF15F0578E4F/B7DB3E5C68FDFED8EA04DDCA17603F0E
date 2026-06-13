using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLab.WebForms.Models
{
    public static class SubmissionRepository
    {
        private static readonly List<Submission> _store = new List<Submission>();

        public static void Save(Submission submission)
        {
            // Id and SubmittedAt are already set by the model's initializers,
            // but you can override here if needed
            _store.Add(submission);
        }

        public static IReadOnlyList<Submission> GetAll() => _store.AsReadOnly();

        public static Submission GetById(Guid id) =>
            _store.Find(s => s.Id == id);
    }
}