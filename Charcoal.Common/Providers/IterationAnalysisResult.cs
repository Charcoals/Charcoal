using System;
using System.Collections.Generic;
using Charcoal.Common.Entities;

namespace Charcoal.Common.Providers
{
    public class IterationAnalysisResult
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }

        public List<IterationResultItem> Items { get; set; }
        public int? NeededAverageVelocity { get; set;}

        public IterationAnalysisResult()
        {
            Items = new List<IterationResultItem>();
        }
    }

    public class IterationResultItem
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int FeaturesAccepted { get; set; }
        public int BugsFixed { get; set; }
        public int BugsAdded { get; set; }
        public int FeaturesAdded { get; set; }
        public int TotalPointsCompleted { get; set; }
        public int? Velocity { get; set; }
        public List<Story> CachedStories { get; set; }
    }
}