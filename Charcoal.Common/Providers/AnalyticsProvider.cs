using System;
using System.Collections.Generic;
using System.Linq;
using Charcoal.Common.Entities;

namespace Charcoal.Common.Providers
{
    public interface IAnalyticsProvider
    {
        OverviewAnalysisResult AnalyzeProject(long projectId, Predicate<Story> unplannedStoriesPoints = null);
        OverviewAnalysisResult AnalyzeStoryTag(long projectId, string tag, Predicate<Story> unplannedStoriesPoints = null);
        IterationAnalysisResult CreateReleaseProjection(OverviewAnalysisResult overviewAnalysis, DateTime targetDate, int iterationlength, DateTime from);
        IterationAnalysisResult AnalyzeRecentIterations(long projectId);
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

            var result = BuildOverViewResult(unplannedStoriesPoints, stories);
            result.ProjectId = projectId;
            result.CachedStories = stories;
            return result;
        }

        public OverviewAnalysisResult AnalyzeStoryTag(long projectId, string tag, Predicate<Story> unplannedStoriesPoints = null)
        {
            var stories = m_storyProvider.GetAllStoriesByTag(projectId, tag)
                                         .Where(e => e.StoryType != StoryType.Chore).ToList();

            var result = BuildOverViewResult(unplannedStoriesPoints, stories);
            result.Name = tag;
            result.ProjectId = projectId;
            result.CachedStories = stories;
            result.IsTagAnalysis = true;
            return result;
        }

        public IterationAnalysisResult CreateReleaseProjection(OverviewAnalysisResult overviewAnalysis, DateTime targetDate, int iterationlength, DateTime from)
        {
            var result = new IterationAnalysisResult
            {
                Name = overviewAnalysis.Name,
                ProjectId = overviewAnalysis.ProjectId,
            };

            var iterationSpan = new TimeSpan(7 * iterationlength, 0, 0, 0);

            if (overviewAnalysis.CachedStories.Any())
            {
                while (from + iterationSpan <= targetDate)
                {
                    var iterationStories = overviewAnalysis.CachedStories.Where(e =>
                        IsInRange(e, from, iterationSpan));

                    if (iterationStories.Any())
                    {
                        result.Items.Add(CreateTimeFramedResult(iterationStories.ToList(),
                                        from, from + iterationSpan));

                    }
                    from += iterationSpan;
                }

                var remainingSpan = (targetDate - from).Days / iterationSpan.Days * 1m;
                if (remainingSpan > 0)
                {
                    result.NeededAverageVelocity = (int)Math.Round(overviewAnalysis.TotalPointsLeft / remainingSpan);
                }
                else
                {
                    result.NeededAverageVelocity = overviewAnalysis.TotalPointsLeft;
                }
            }

            return result;
        }

        public IterationAnalysisResult AnalyzeRecentIterations(long projectId)
        {
            var result = new IterationAnalysisResult
            {
                Name = "Recent Iteration review",
                ProjectId = projectId,
            };

            var iterations = m_storyProvider.GetRecentIterations(projectId, 10);

            foreach (var iteration in iterations)
            {
                result.Items.Add(CreateTimeFramedResult(iteration.Stories, iteration.Start.Value, iteration.Finish.Value));
            }
            return result;
        }


        static IterationResultItem CreateTimeFramedResult(List<Story> stories, DateTime iterationStart, DateTime iterationEnd)
        {
            var result = new IterationResultItem();
            result.CachedStories = stories;
            result.From = iterationStart;
            result.To = iterationEnd;
            var enumerable = stories.Where(e => e.Status== StoryStatus.Accepted && e.Estimate.HasValue && InRange(e.AcceptedOn.Value, iterationStart,iterationEnd));
            result.TotalPointsCompleted = enumerable
                                                    .Sum(e => e.Estimate.Value);

            result.BugsFixed = stories.Count(e => e.StoryType == StoryType.Bug
                                                 && IsCompleted(e.Status));
            result.FeaturesAccepted = stories.Count(e => e.StoryType == StoryType.Feature &&
                                                       e.Status== StoryStatus.Accepted);

            result.BugsAdded = stories.Count(e => e.StoryType == StoryType.Bug
                                                    && InRange(e.CreatedOn, iterationStart,
                                                                iterationEnd));
            result.FeaturesAdded = stories.Count(e => e.StoryType == StoryType.Feature
                                                        && InRange(e.CreatedOn, iterationStart,
                                                                    iterationEnd));
            return result;


        }

        static bool IsInRange(Story story, DateTime iterationStart, TimeSpan span)
        {
            var iterationEnd = iterationStart + span;
            return (InRange(story.CreatedOn, iterationStart, iterationEnd)
                   || (story.AcceptedOn.HasValue
                       && InRange(story.AcceptedOn.Value, iterationStart, iterationEnd)));
        }

        static bool InRange(DateTime time, DateTime from, DateTime to)
        {
            return time >= from && time < to;
        }

        private OverviewAnalysisResult BuildOverViewResult(Predicate<Story> unplannedStoriesPoints, List<Story> stories)
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
}