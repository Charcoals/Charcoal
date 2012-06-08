using System;
using System.Collections.Generic;
using System.Linq;
using Charcoal.Common.Entities;

namespace Charcoal.Common.Providers
{
    public interface IAnalyticsProvider
    {
        OverviewAnalysisResult AnalyzeProject(long projectId, Predicate<Story> unplannedStoriesPoints = null);
        OverviewAnalysisResult AnalyzeStoryLabel(long projectId,string label, Predicate<Story> unplannedStoriesPoints = null);
    }

    public class AnalyticsProvider : IAnalyticsProvider
    {
        private readonly IStoryProvider m_storyProvider;
        private readonly int? m_projectVelocity;

        public AnalyticsProvider(IStoryProvider storyProvider, int? projectVelocity = null)
        {
            m_storyProvider = storyProvider;
            m_projectVelocity = projectVelocity;
        }

        public OverviewAnalysisResult AnalyzeProject(long projectId, Predicate<Story> unplannedStoriesPoints = null)
        {
            var stories = m_storyProvider.GetAllStories(projectId)
                                         .Where(e => e.StoryType != StoryType.Chore).ToList();

            return BuildResult(unplannedStoriesPoints, stories);
        }

        public OverviewAnalysisResult AnalyzeStoryLabel(long projectId, string label, Predicate<Story> unplannedStoriesPoints = null)
        {
            var stories = m_storyProvider.GetAllStories(projectId)
                                         .Where(e => e.StoryType != StoryType.Chore).ToList();

            return BuildResult(unplannedStoriesPoints, stories);
        }

        private OverviewAnalysisResult BuildResult(Predicate<Story> unplannedStoriesPoints, List<Story> stories)
        {
            var result = new OverviewAnalysisResult();
            result.Velocity = m_projectVelocity;
            if (unplannedStoriesPoints != null)
            {
                result.UnplannedStoriesPoints =
                    stories.Where(e => unplannedStoriesPoints(e) && e.Estimate.HasValue).Sum(e => e.Estimate.Value);
            }


            var bugs = stories.Where(e => e.StoryType == StoryType.Bug);
            result.TotalBugsCount = bugs.Count();
            result.RemainingBugsCount = bugs.Count(e => !IsCompleted(e.Status));

            result.FeaturesCount = stories.Count(e => e.StoryType == StoryType.Feature);
            result.UnestimatedStoriesCount = stories.Count(e => !IsEstimated(e));
            result.TotalPointsCompleted = stories.Where(e => IsEstimated(e)
                                                             && IsCompleted(e.Status))
                .Sum(e => e.Estimate.Value);

            result.TotalPointsLeft = stories.Where(e => IsEstimated(e) && !IsCompleted(e.Status))
                .Sum(e => e.Estimate.Value);

            return result;
        }

        private static bool IsEstimated(Story e)
        {
            return e.Estimate.HasValue && e.Estimate >= 0;
        }

        private static bool IsCompleted(StoryStatus status)
        {
            return status == StoryStatus.Finished
                || status == StoryStatus.Delivered
                || status == StoryStatus.Accepted;
        }
    }

    public class OverviewAnalysisResult
    {
        public string Name { get; set; }
        public int FeaturesCount { get; set; }
        public int? UnplannedStoriesPoints { get; set; }
        public int UnestimatedStoriesCount { get; set; }
        public int? Velocity { get; set; }
        public int TotalPointsCompleted { get; set; }
        public int TotalPointsLeft { get; set; }
        public int TotalBugsCount { get; set; }
        public int RemainingBugsCount { get; set; }

    }
}