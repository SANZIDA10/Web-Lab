using System.Collections.Generic;

namespace WebLab.WebForms.Models
{
    public sealed class MemberStatistics
    {
        public int TotalMembers { get; }

        public int JoinedThisMonth { get; }

        public IReadOnlyDictionary<string, int> ProgramDistribution { get; } = new Dictionary<string, int>();
    }
}