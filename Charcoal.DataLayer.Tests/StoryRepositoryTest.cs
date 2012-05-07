using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
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
        }

        [TearDown]
        public void Cleanup()
        {
            m_database.Tasks.DeleteAll();
            m_database.Stories.DeleteAll();
        }

        [Test]
        public void CanSaveStory()
        {
            dynamic story = new ExpandoObject();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = 3;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            var stories = m_database.Stories.All().ToList<dynamic>();
            Assert.AreEqual(1, stories.Count);

            VerifyStory(story, stories[0]);
        }

        [Test]
        public void CanUpdateExistingStory()
        {
            dynamic story = new ExpandoObject();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = 3;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            var retrievedStory = m_database.Stories.All().ToList<dynamic>()[0];
            retrievedStory.Title = "New Title";

            response = m_repository.Update(retrievedStory);
            Assert.IsTrue(response.HasSucceeded);

            var stories = m_database.Stories.All().ToList<dynamic>();
            Assert.AreEqual(1, stories.Count);

            VerifyStory(retrievedStory, stories[0]);
        }

        [Test]
        public void CannotUpdateNonExisitingStory()
        {
            dynamic story = new ExpandoObject();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = 3;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;

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
            dynamic story = new ExpandoObject();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = 3;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            var retrievedStory = m_database.Stories.All().ToList<dynamic>()[0];

            response = m_repository.Delete(retrievedStory.Id);
            Assert.IsTrue(response.HasSucceeded);

            var stories = m_database.Stories.All().ToList<dynamic>();
            Assert.AreEqual(0, stories.Count);
        }

        [Test]
        public void CanFindAll()
        {
            dynamic story = new ExpandoObject();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = 3;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

            var repository = new TaskRepository(DatabaseHelper.GetConnectionString());
            dynamic task = new ExpandoObject();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            response = repository.Save(task);
            Assert.IsTrue(response.HasSucceeded, response.Description);

            dynamic task2 = new ExpandoObject();
            task2.Description = "Yes I am a taks";
            task2.Assignees = "Dude1, Dude2";
            task2.IsCompleted = true;
            task2.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;


            response = repository.Save(task2);
            Assert.IsTrue(response.HasSucceeded, response.Description);

            var stories = m_repository.FindAll();
            Assert.AreEqual(1, stories.Count);
            Assert.AreEqual(2, stories.Single().Tasks.Count);
        }

        static void VerifyStory(dynamic expected, dynamic actual)
        {
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Status, actual.Status);
        }
    }
}
