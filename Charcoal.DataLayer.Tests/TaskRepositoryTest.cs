using System.Dynamic;
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

            var storyRepository = new StoryRepository(DatabaseHelper.GetConnectionString());
            dynamic story = new ExpandoObject();
            story.Title = "My New story";
            story.Description = "loooooooo";
            story.Status = 3;
            story.CreatedBy = m_database.Users.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = storyRepository.Save(story);
            Assert.IsTrue(response.HasSucceeded);
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
        }

        [Test]
        public void CanSaveTask()
        {
            dynamic task = new ExpandoObject();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(task);
            Assert.IsTrue(response.HasSucceeded);

            var tasks = m_database.Tasks.All().ToList<dynamic>();
            Assert.AreEqual(1, tasks.Count);
            Verifytask(task, tasks[0]);
        }

        [Test]
        public void CanFindById()
        {
            dynamic task = new ExpandoObject();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(task);
            Assert.IsTrue(response.HasSucceeded, response.Description);

            var retrievedTask = m_database.Tasks.All().ToList<dynamic>()[0];


            var foundTask = m_repository.Find(retrievedTask.Id);
            Verifytask(retrievedTask, foundTask);

            Assert.NotNull(foundTask.Stories);
            Assert.NotNull(foundTask.Stories.Id);
            Assert.AreEqual(task.StoryId, foundTask.Stories.Id);
        }

        [Test]
        public void CannotUpdateInexistantTask()
        {
            dynamic task = new ExpandoObject();
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
            dynamic task = new ExpandoObject();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(task);
            Assert.IsTrue(response.HasSucceeded);

            var retrievedTask = m_database.Tasks.All().ToList<dynamic>()[0];
            retrievedTask.IsCompleted = false;
            task.Description = "blah blah";

            response = m_repository.Update(retrievedTask);
            Assert.IsTrue(response.HasSucceeded);

            var tasks = m_database.Tasks.All().ToList<dynamic>();
            Assert.AreEqual(1, tasks.Count);
            Verifytask(retrievedTask, tasks[0]);
        }

        [Test]
        public void CanDeleteTask()
        {
            dynamic task = new ExpandoObject();
            task.Description = "Im a task";
            task.Assignees = "Dude1, Dude2";
            task.IsCompleted = true;
            task.StoryId = m_database.Stories.All().ToList<dynamic>()[0].Id;

            DatabaseOperationResponse response = m_repository.Save(task);
            Assert.IsTrue(response.HasSucceeded);

            var retrievedTask = m_database.Tasks.All().ToList<dynamic>()[0];

            response = m_repository.Delete(retrievedTask.Id);
            Assert.IsTrue(response.HasSucceeded);

            var tasks = m_database.Tasks.All().ToList<dynamic>();
            Assert.AreEqual(0, tasks.Count);
        }

        static void Verifytask(dynamic expected, dynamic actual)
        {
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Assignees, actual.Assignees);
            Assert.AreEqual(expected.StoryId, actual.StoryId);
            Assert.AreEqual(expected.IsCompleted, actual.IsCompleted);
        }

    }
}