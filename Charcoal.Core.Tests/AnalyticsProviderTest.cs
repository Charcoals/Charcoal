using System.Linq;
using Charcoal.Common.Entities;
using Charcoal.Common.Providers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Charcoal.Core.Tests
{
    [TestFixture]
    public class AnalyticsProviderTest
    {
        [Test]
        public void CanAnalyzeProject()
        {
            var storyProvider = new Mock<IStoryProvider>(MockBehavior.Strict);
            var feature1 = new Story
            {
                Estimate = 2,
                Status = StoryStatus.Accepted,
                StoryType = StoryType.Feature
            };
            var feature2 = new Story
            {
                Estimate = 2,
                Status = StoryStatus.Finished,
                StoryType = StoryType.Feature
            };
            var feature3 = new Story
            {
                Estimate = 2,
                Status = StoryStatus.Delivered,
                StoryType = StoryType.Feature
            };

            var bug1 = new Story
            {
                Estimate = 2,
                Status = StoryStatus.Accepted,
                StoryType = StoryType.Bug
            };

            var bug2 = new Story
            {
                Estimate = -1,
                Status = StoryStatus.UnScheduled,
                StoryType = StoryType.Bug
            };

            var unschedueldFeature = new Story
            {
                Estimate = 2,
                Status = StoryStatus.UnScheduled,
                StoryType = StoryType.Feature
            };
            var rejectedFeature = new Story
            {
                Estimate = 100,
                Status = StoryStatus.Rejected,
                StoryType = StoryType.Feature
            };

            var unplannedFeature = new Story
            {
                Estimate = 1,
                Status = StoryStatus.Delivered,
                StoryType = StoryType.Feature,
                Description = "mcjawn"
            };
            var rejectedBug = new Story
            {
                Estimate = 100,
                Status = StoryStatus.Rejected,
                StoryType = StoryType.Bug
            };

            var chore = new Story
            {
                Estimate = 100,
                Status = StoryStatus.Finished,
                StoryType = StoryType.Chore
            };


            var stories = new[] { feature1, feature2, 
                feature3, bug1,bug2, unschedueldFeature,unplannedFeature, 
                    rejectedBug, rejectedFeature, chore }
                    .ToList();

            var project = new {Id = 212, Velocity = 3};
            storyProvider.Setup(e => e.GetAllStories(project.Id)).Returns(stories);


            var result = new AnalyticsProvider(storyProvider.Object, project.Velocity)
                                               .AnalyzeProject(project.Id, j => j.Description.Equals("mcjawn"));
            
            Assert.AreEqual(project.Velocity,result.Velocity.Value);
            Assert.AreEqual(6, result.FeaturesCount);
            Assert.AreEqual(9,result.TotalPointsCompleted);
            Assert.AreEqual(202, result.TotalPointsLeft);
            Assert.AreEqual(1, result.UnestimatedStoriesCount);
            Assert.AreEqual(1, result.UnplannedStoriesPoints);
            Assert.AreEqual(3, result.TotalBugsCount);
            Assert.AreEqual(2, result.RemainingBugsCount);

            storyProvider.Verify();

        }

        [Test]
        public void CanAnalyzeProject_UnplannedStoriesNotFound()
        {
            var storyProvider = new Mock<IStoryProvider>(MockBehavior.Strict);
            
            var unplannedFeature = new Story
            {
                Estimate = 1,
                Status = StoryStatus.Delivered,
                StoryType = StoryType.Feature,
            };
            
            var project = new { Id = 212, Velocity = 3 };
            storyProvider.Setup(e => e.GetAllStories(project.Id)).Returns(new List<Story> { unplannedFeature });


            var result = new AnalyticsProvider(storyProvider.Object, project.Velocity)
                                               .AnalyzeProject(project.Id, j => j.Description.Equals("mcjawn"));

            Assert.AreEqual(0, result.UnplannedStoriesPoints);

            storyProvider.Verify();
        }
    }
}