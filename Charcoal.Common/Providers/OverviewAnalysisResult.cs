using System.Collections.Generic;
using Charcoal.Common.Entities;

namespace Charcoal.Common.Providers
{
    public class OverviewAnalysisResult
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public List<Story> CachedStories { get; set; } 

        public int FeaturesCount { get; set; }
        public int? UnplannedStoriesPoints { get; set; }
        public int UnestimatedStoriesCount { get; set; }
        public int? Velocity { get; set; }
        public int TotalPointsCompleted { get; set; }
        public int TotalPointsLeft { get; set; }
        public int TotalBugsCount { get; set; }
        public int RemainingBugsCount { get; set; }
        public bool IsTagAnalysis { get; set; }
    }
}