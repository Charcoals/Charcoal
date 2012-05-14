using Charcoal.Core.Entities;
using NUnit.Framework;
using Simple.Data;

namespace Charcoal.DataLayer.Tests
{
    [TestFixture]
    public class ProjectRepositoryTest
    {
        private dynamic m_database;
        private ProjectRepository m_repository;

        [TestFixtureSetUp]
        public void Init()
        {
            m_database = Database.OpenConnection(DatabaseHelper.GetConnectionString());
            m_repository = new ProjectRepository(DatabaseHelper.GetConnectionString());
        }

        [TearDown]
        public void CleanUp()
        {
            m_database.Projects.DeleteAll();
            m_database.Users.DeleteAll();
        }

        [Test]
        public void CanSaveProject()
        {
            var project = new Project();
            project.Title = "loooooooo";
            project.Description = "loooe3rewrrewooooo";

            DatabaseOperationResponse response = m_repository.Save(project);
            Assert.IsTrue(response.HasSucceeded);

            var users = m_database.Projects.All().ToList<Project>();
            Assert.AreEqual(1, users.Count);

            VerifyProject(project, users[0]);
        }

        [Test]
        public void CanUpdateExistingProject()
        {
            var project = new Project();
            project.Title = "loooooooo";
            project.Description = "loooe3rewrrewooooo";

            DatabaseOperationResponse response = m_repository.Save(project);
            Assert.IsTrue(response.HasSucceeded);

            Project retrievedProject = m_database.Projects.All().ToList<Project>()[0];
            retrievedProject.Title = "New Title";

            response = m_repository.Update(retrievedProject);
            Assert.IsTrue(response.HasSucceeded);

            var projects = m_database.Projects.All().ToList<Project>();
            Assert.AreEqual(1, projects.Count);

            VerifyProject(retrievedProject, projects[0]);
        }

        [Test]
        public void CanDeleteExistingProject()
        {
            var project = new Project();
            project.Title = "loooooooo";
            project.Description = "loooe3rewrrewooooo";

            DatabaseOperationResponse response = m_repository.Save(project);
            Assert.IsTrue(response.HasSucceeded);

            Project retrievedProject = m_database.Projects.All().ToList<Project>()[0];
            response = m_repository.Delete(retrievedProject.Id);

            Assert.IsTrue(response.HasSucceeded);

            var projects = m_database.Projects.All().ToList<User>();
            Assert.AreEqual(0, projects.Count);
        }

        [Test]
        public void CanFindById()
        {
            var project = new Project();
            project.Title = "loooooooo";
            project.Description = "loooe3rewrrewooooo";

            DatabaseOperationResponse response = m_repository.Save(project);
            Assert.IsTrue(response.HasSucceeded);

            Project retrievedProject = m_database.Projects.All().ToList<Project>()[0];

            Project foundUser = m_repository.Find(retrievedProject.Id);
            VerifyProject(retrievedProject, foundUser);
        }

        [Test]
        public void CanFindAll()
        {
            var project = new Project();
            project.Title = "loooooooo";
            project.Description = "loooe3rewrrewooooo";

            DatabaseOperationResponse response = m_repository.Save(project);
            Assert.IsTrue(response.HasSucceeded);

            Project retrievedProject = m_database.Projects.All().ToList<Project>()[0];

            var foundUsers = m_repository.FindAll();
            Assert.AreEqual(1, foundUsers.Count);
            VerifyProject(retrievedProject, foundUsers[0]);
        }



        private void VerifyProject(dynamic expected, dynamic actual)
        {
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Description, actual.Description);
        }


    }
}