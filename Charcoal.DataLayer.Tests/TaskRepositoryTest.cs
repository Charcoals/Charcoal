using System;
using System.Dynamic;
using Charcoal.Common.Entities;
using NUnit.Framework;
using Simple.Data;

namespace Charcoal.DataLayer.Tests
{
    [TestFixture]
    public class TaskRepositoryTest
    {
        private TaskRepository m_repository;
        private dynamic m_database;

        [TestFixtureSetUp]
        public void Init()
        {
            m_repository = new TaskRepository(DatabaseHelper.GetConnectionString());
            m_database = Database.OpenConnection(DatabaseHelper.GetConnectionString());
            m_database.Users.Insert(UserName: "somedude", FirstName: "Some", LastName: "Dude", APIKey: "yuiu-998",
            Email: "aaa@aaa.com", Privileges: 2, Password:"jawn");
            m_database.Projects.Insert(Title: "wololo", Description: "blah");

            dynamic story = new ExpandoObject();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = 3;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;
            story.CreatedOn = DateTime.UtcNow;
            story.LastEditedOn = DateTime.UtcNow;
            story.ProjectId = m_database.Projects.All().ToList<dynamic>()[0].Id;
            

            m_database.Stories.Insert(story);
        }

        [TearDown]
        public void Cleanup()
        {
            m_database.Tasks.DeleteAll();
        }

        [TestFixtureTearDown]
        public void StoriesCleanUp()
        {
            m_database.Stories.DeleteAll();
            m_database.Projects.DeleteAll();
            m_database.Users.DeleteAll();
        }

        [Test]
        public void CanSaveTask()
        {
            var task = new Task();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;
            task.Position = 5;
            DatabaseOperationResponse response = m_repository.Save(task);
            Assert.IsTrue(response.HasSucceeded);

            var tasks = m_database.Tasks.All().ToList<Task>();
            Assert.AreEqual(1, tasks.Count);
            Verifytask(task, tasks[0]);
        }

        [Test]
        public void CanFindById()
        {
            var task = new Task();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(task);
            Assert.IsTrue(response.HasSucceeded, response.Description);

            Task retrievedTask = m_database.Tasks.All().ToList<dynamic>()[0];


            Task foundTask = m_repository.Find(retrievedTask.Id);
            Verifytask(retrievedTask, foundTask);

            Assert.NotNull(foundTask.Story);
            Assert.NotNull(foundTask.Story.Id);
            Assert.AreEqual(task.StoryId, foundTask.Story.Id);
        }

        [Test]
        public void CannotUpdateInexistantTask()
        {
            var task = new Task();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Update(task);
            Assert.IsFalse(response.HasSucceeded);
        }

        [Test]
        public void CannotDeleteInexistantTask()
        {
          
            DatabaseOperationResponse response = m_repository.Delete(4);
            Assert.IsFalse(response.HasSucceeded);
        }

        [Test]
        public void CanUpdateTask()
        {
            var task = new Task();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(task);
            Assert.IsTrue(response.HasSucceeded);

            Task retrievedTask = m_database.Tasks.All().ToList<dynamic>()[0];
            retrievedTask.IsCompleted = false;
            task.Description = "blah blah";

            response = m_repository.Update(retrievedTask);
            Assert.IsTrue(response.HasSucceeded);

            var tasks = m_database.Tasks.All().ToList<Task>();
            Assert.AreEqual(1, tasks.Count);
            Verifytask(retrievedTask, tasks[0]);
        }

        [Test]
        public void CanDeleteTask()
        {
            var task = new Task();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(task);
            Assert.IsTrue(response.HasSucceeded);

            Task retrievedTask = m_database.Tasks.All().ToList<dynamic>()[0];

            response = m_repository.Delete(retrievedTask.Id);
            Assert.IsTrue(response.HasSucceeded);

            var tasks = m_database.Tasks.All().ToList<Task>();
            Assert.AreEqual(0, tasks.Count);
        }

        static void Verifytask(dynamic expected, dynamic actual)
        {
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Assignees, actual.Assignees);
            Assert.AreEqual(expected.StoryId, actual.StoryId);
            Assert.AreEqual(expected.IsCompleted, actual.IsCompleted);
            Assert.AreEqual(expected.Position, actual.Position);
        }

    }
}