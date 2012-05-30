using System.Linq;
using Charcoal.Common.Entities;
using NUnit.Framework;
using Simple.Data;

namespace Charcoal.DataLayer.Tests
{
    [TestFixture]
    public class StoryRepositoryTest
    {
        private StoryRepository m_repository;
        private dynamic m_database;

        [TestFixtureSetUp]
        public void Init()
        {
            m_repository = new StoryRepository(DatabaseHelper.GetConnectionString());
            m_database = Database.OpenConnection(DatabaseHelper.GetConnectionString());

            m_database.Users.Insert(UserName: "somedude", FirstName: "Some", LastName: "Dude", APIKey: "yuiu-998",
            Email: "aaa@aaa.com", Privileges: 2, Password:"lolll");
            m_database.Projects.Insert(Title: "wololo", Description: "blah");
        }

        [TearDown]
        public void Cleanup()
        {
            m_database.Tasks.DeleteAll();
            m_database.Stories.DeleteAll();
        }

        [TestFixtureTearDown]
        public void FullCleanUp()
        {
            m_database.Projects.DeleteAll();
            m_database.Users.DeleteAll();
        }

        [Test]
        public void CanSaveStory()
        {
            var story = new Story();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = StoryStatus.Started;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;
            story.ProjectId = m_database.Projects.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            var stories = m_database.Stories.All().ToList<Story>();
            Assert.AreEqual(1, stories.Count);

            VerifyStory(story, stories[0]);
            var responseObject = (Story) response.Object;
            VerifyStory(responseObject, stories[0]);
            Assert.IsTrue(responseObject.Id >0);
        }


        [Test]
        public void CanUpdateExistingStory()
        {
            var story = new Story();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = StoryStatus.Started;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;
            story.ProjectId = m_database.Projects.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            Story retrievedStory = m_database.Stories.All().ToList<dynamic>()[0];
            retrievedStory.Title = "New Title";

            response = m_repository.Update(retrievedStory);
            Assert.IsTrue(response.HasSucceeded);

            var stories = m_database.Stories.All().ToList<Story>();
            Assert.AreEqual(1, stories.Count);

            VerifyStory(retrievedStory, stories[0]);
        }

        [Test]
        public void CanUpdateStoryIterationType()
        {
            var story = new Story();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = StoryStatus.Started;
            story.IterationType= IterationType.Current;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;
            story.ProjectId = m_database.Projects.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            Story retrievedStory = response.Object;


            var storyStatus = StoryStatus.Accepted;
            Story updatedStory = m_repository.UpdateStoryStatus(retrievedStory.Id, (int)storyStatus);
            
            Assert.AreEqual(storyStatus,updatedStory.Status);
        }

        [Test]
        public void CannotUpdateNonExisitingStory()
        {
            var story = new Story();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = StoryStatus.Started;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;
            story.ProjectId = m_database.Projects.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Update(story);
            Assert.IsFalse(response.HasSucceeded);
        }

        [Test]
        public void CannotDeleteNonExisitingStory()
        {

            DatabaseOperationResponse response = m_repository.Delete(78);
            Assert.IsFalse(response.HasSucceeded);
        }

        [Test]
        public void CanDeleteExistingStory()
        {
            var story = new Story();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = StoryStatus.Started;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;
            story.ProjectId = m_database.Projects.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            Story retrievedStory = m_database.Stories.All().ToList<Story>()[0];

            response = m_repository.Delete(retrievedStory.Id);
            Assert.IsTrue(response.HasSucceeded);

            var stories = m_database.Stories.All().ToList<Story>();
            Assert.AreEqual(0, stories.Count);
        }

        [Test]
        public void CanFindAllByIterationType()
        {
            var story = new Story();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = StoryStatus.Started;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;
            story.ProjectId = m_database.Projects.All().ToList<dynamic>()[0].Id;
            story.IterationType = IterationType.Icebox;
            
            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            var repository = new TaskRepository(DatabaseHelper.GetConnectionString());
            var task = new Task();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            response = repository.Save(task);
            Assert.IsTrue(response.HasSucceeded, response.Description);

            var task2 = new Task();
            task2.Description = "Yes I am a taks";
            task2.Assignees = "Dude1, Dude2";
            task2.IsCompleted = true;
            task2.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;


            response = repository.Save(task2);
            Assert.IsTrue(response.HasSucceeded, response.Description);

            var stories = m_repository.FindAllByIterationType(story.ProjectId, (int)story.IterationType);
            Assert.AreEqual(1, stories.Count);
            Assert.AreEqual(2, stories[0].Tasks.Count);
            Assert.NotNull(stories[0].Project);
            Assert.AreEqual(story.ProjectId, stories[0].Project.Id);
        }

        [Test]
        public void CanFindAllByProjectId()
        {
            var story = new Story();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = StoryStatus.Started;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;
            story.ProjectId = m_database.Projects.All().ToList<dynamic>()[0].Id;
            story.IterationType = IterationType.Icebox;

            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            var repository = new TaskRepository(DatabaseHelper.GetConnectionString());
            var task = new Task();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            response = repository.Save(task);
            Assert.IsTrue(response.HasSucceeded, response.Description);

            var task2 = new Task();
            task2.Description = "Yes I am a taks";
            task2.Assignees = "Dude1, Dude2";
            task2.IsCompleted = true;
            task2.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;


            response = repository.Save(task2);
            Assert.IsTrue(response.HasSucceeded, response.Description);

            var stories = m_repository.FindAllByProjectId(story.ProjectId);
            Assert.AreEqual(1, stories.Count);
            Assert.AreEqual(2, stories[0].Tasks.Count);
            Assert.NotNull(stories[0].Project);
            Assert.AreEqual(story.ProjectId, stories[0].Project.Id);
        }

        [Test]
        public void CanFindAll()
        {
            var story = new Story();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = StoryStatus.Started;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;
            story.ProjectId = m_database.Projects.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            var repository = new TaskRepository(DatabaseHelper.GetConnectionString());
            var task = new Task();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            response = repository.Save(task);
            Assert.IsTrue(response.HasSucceeded, response.Description);

            var task2 = new Task();
            task2.Description = "Yes I am a taks";
            task2.Assignees = "Dude1, Dude2";
            task2.IsCompleted = true;
            task2.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;


            response = repository.Save(task2);
            Assert.IsTrue(response.HasSucceeded, response.Description);

            var stories = m_repository.FindAll();
            Assert.AreEqual(1, stories.Count);
            Assert.AreEqual(2, stories[0].Tasks.Count);
            Assert.NotNull(stories[0].Project);
            Assert.AreEqual(story.ProjectId, stories[0].Project.Id);
        }

        static void VerifyStory(Story expected, Story actual)
        {
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Status, actual.Status);
        }
    }
}
