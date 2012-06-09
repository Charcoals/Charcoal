using System;
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

        [Test]
        public void CanAnalyzeProjectLabel()
        {
            var storyProvider = new Mock<IStoryProvider>(MockBehavior.Strict);
            var feature1 = new Story
            {
                Estimate = 2,
                Status = StoryStatus.Accepted,
                StoryType = StoryType.Feature,
                Tag = "tag"
                
            };
           
            var project = new { Id = 212, Velocity = 3 };
            storyProvider.Setup(e => e.GetAllStoriesByTag(project.Id, feature1.Tag)).Returns(new List<Story> { feature1});


            var result = new AnalyticsProvider(storyProvider.Object, project.Velocity)
                                               .AnalyzeStoryTag(project.Id, feature1.Tag, j => j.Description.Equals("mcjawn"));

            Assert.AreEqual(project.Velocity, result.Velocity.Value);
            Assert.AreEqual(1, result.FeaturesCount);
            Assert.AreEqual(2, result.TotalPointsCompleted);
            
            storyProvider.Verify();
        }

        [Test]
        public void CanProjectTheRelease()
        {
            var dateTime = new DateTime(2001, 3, 1, 3, 44, 12);

            var week1 = CreateStories(dateTime);
            var week2 = CreateStories(dateTime.AddDays(7));
            var week3 = CreateStories(dateTime.AddDays(17));

            var result = new AnalyticsProvider(null).CreateReleaseProjection(
                new OverviewAnalysisResult
                    {
                        CachedStories =  week1.Concat(week2).Concat(week3)
                    },
                dateTime.AddDays(7*4),
                2,
                dateTime
                );
            
            Assert.AreEqual(2, result.Items.Count);

            var firstResult = result.Items.First();
            var firstStories = firstResult.CachedStories;

            Assert.AreEqual(10, firstStories.Count());
            Assert.AreEqual(5, firstStories.Count(e => e.StoryType == StoryType.Feature));
            Assert.AreEqual(5, firstStories.Count(e => e.StoryType == StoryType.Bug));
            Assert.AreEqual(4, firstResult.FeaturesAccepted);
            Assert.AreEqual(4, firstResult.BugsFixed);

            Assert.AreEqual(4, firstResult.FeaturesAdded);
            Assert.AreEqual(4, firstResult.BugsAdded);

            Assert.AreEqual(8, firstResult.TotalPointsCompleted);

            var secondResult = result.Items.ElementAt(1);
            var secondStories = secondResult.CachedStories;

            Assert.AreEqual(6, secondStories.Count());
            Assert.AreEqual(3, secondStories.Count(e => e.StoryType == StoryType.Feature));
            Assert.AreEqual(3, secondStories.Count(e => e.StoryType == StoryType.Bug));
            Assert.AreEqual(2, secondResult.FeaturesAccepted);
            Assert.AreEqual(2, secondResult.BugsFixed);

            Assert.AreEqual(4, secondResult.TotalPointsCompleted);

            Assert.AreEqual(3, secondResult.FeaturesAdded);
            Assert.AreEqual(3, secondResult.BugsAdded);

        }

        static IEnumerable<Story> CreateStories(DateTime date)
        {
            var feature1 = new Story
            {
                AcceptedOn = date,
                CreatedOn = date.AddDays(-1),
                Estimate = 1,
                StoryType = StoryType.Feature,
                Status = StoryStatus.Accepted
            };

            var feature2 = new Story
            {
                CreatedOn = date.AddDays(-1),
                Estimate = 1,
                StoryType = StoryType.Feature,
                Status = StoryStatus.Finished
            };

            var feature3 = new Story
            {
                AcceptedOn = date,
                CreatedOn = date.AddDays(1),
                Estimate = 1,
                StoryType = StoryType.Feature,
                Status = StoryStatus.Accepted
            };

            var bug1 = new Story
            {
                AcceptedOn = date,
                CreatedOn = date.AddDays(-1),
                Estimate = 1,
                StoryType = StoryType.Bug,
                Status = StoryStatus.Accepted
            };

            var bug2 = new Story
            {
                CreatedOn = date.AddDays(-1),
                Estimate = 1,
                StoryType = StoryType.Bug,
                Status = StoryStatus.Finished
            };

            var bug3 = new Story
            {
                AcceptedOn = date,
                CreatedOn = date.AddDays(1),
                Estimate = 1,
                StoryType = StoryType.Bug,
                Status = StoryStatus.Accepted
            };

            return new []{feature1, feature2, feature3, bug1, bug2, bug3};
        }


    }
}