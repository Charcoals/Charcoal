using System.Collections.Generic;
using Charcoal.Common.Entities;
using Charcoal.DataLayer;
using Moq;
using NUnit.Framework;

namespace Charcoal.Core.Tests
{
    [TestFixture]
    public class CharcoalStoryProviderTest
    {
        const long projectId = 99;

        [Test]
        public void CanAddNewStory()
        {
            var story = new Story();
            var storyRepo = new Mock<IStoryRepository>(MockBehavior.Strict);
            storyRepo.Setup(repo => repo.Save(It.Is<Story>(e => e.ProjectId == projectId)))
                      .Returns(new DatabaseOperationResponse(true));

            new CharcoalStoryProvider(storyRepo.Object).AddNewStory(projectId, story);
            storyRepo.Verify();
        }

        [Test]
        public void CanAddNewStoryFails()
        {
            var story = new Story();
            var storyRepo = new Mock<IStoryRepository>(MockBehavior.Strict);
            storyRepo.Setup(repo => repo.Save(It.Is<Story>(e => e.ProjectId == projectId)))
                      .Returns(new DatabaseOperationResponse());

            Assert.IsNull(new CharcoalStoryProvider(storyRepo.Object).AddNewStory(projectId, story));
            storyRepo.Verify();
        }

        [Test]
        public void GetStories()
        {
            var story = new Story {IterationType = IterationType.Backlog};
            var storyRepo = new Mock<IStoryRepository>(MockBehavior.Strict);
            storyRepo.Setup(repo => repo.FindAllByIterationType(projectId,(int)IterationType.Backlog ))
                      .Returns(new List<Story>{story});

            new CharcoalStoryProvider(storyRepo.Object).GetStories(projectId, IterationType.Backlog);
            storyRepo.Verify();
        }

        [Test]
        public void GeAlltStories()
        {
            var story = new Story { IterationType = IterationType.Backlog , ProjectId = projectId};
            var storyRepo = new Mock<IStoryRepository>(MockBehavior.Strict);
            storyRepo.Setup(repo => repo.FindAllByProjectId(projectId))
                      .Returns(new List<Story> { story });

            new CharcoalStoryProvider(storyRepo.Object).GetAllStories(projectId);
            storyRepo.Verify();
        }
    }
}