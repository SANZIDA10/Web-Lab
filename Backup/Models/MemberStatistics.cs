using System.Collections.Generic;

namespace WebLab.WebForms.Models
{
    public sealed class MemberStatistics
    {
        public int TotalMembers { get; init; }

        public int JoinedThisMonth { get; init; }

        public IReadOnlyDictionary<string, int> ProgramDistribution { get; init; } = new Dictionary<string, int>();
    }
}