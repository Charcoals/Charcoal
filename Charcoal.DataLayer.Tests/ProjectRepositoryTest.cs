using System.Collections.Generic;
using Charcoal.Common.Entities;
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
            project.Velocity = 32;
            DatabaseOperationResponse response = m_repository.Save(project);
            Assert.IsTrue(response.HasSucceeded);

            var projects = m_database.Projects.All().ToList<Project>();
            Assert.AreEqual(1, projects.Count);

            VerifyProject(project, projects[0]);
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

            List<Project> projects = m_repository.FindAll().ConvertAll(e=>(Project)e);
            Assert.AreEqual(1, projects.Count);
            VerifyProject(retrievedProject, projects[0]);
        }

        [Test]
        public void CanFindAllByUserToken()
        {
            var project = new Project();
            project.Title = "loooooooo";
            project.Description = "loooe3rewrrewooooo";

            var project2 = new Project();
            project2.Title = "ssss";
            project.Description = "loooe3rewrrewooooo";

            DatabaseOperationResponse response = m_repository.Save(project);
            Assert.IsTrue(response.HasSucceeded);

            response = m_repository.Save(project2);
            Assert.IsTrue(response.HasSucceeded);

            var token = "yuiu-998";
            m_database.Users.Insert(UserName: "somedude", FirstName: "Some", LastName: "Dude", APIKey: token,
            Email: "aaa@aaa.com", Privileges: 2, Password: "lolll");

            List<Project> dbProjects = m_database.Projects.All().ToList<Project>();
            long userId = m_database.Users.All().ToList<User>()[0].Id;

            m_database.UsersXProjects.Insert(UserId: userId, ProjectId: dbProjects[0].Id);
            m_database.UsersXProjects.Insert(UserId: userId, ProjectId: dbProjects[1].Id);

            List<Project> projects = m_repository.GetProjectsByUseToken(token).ConvertAll(e=>(Project)e);
            Assert.AreEqual(2, projects.Count);
        }

        [Test]
        public void CanCreateProjectAssociatedWithKey()
        {
            var project = new Project();
            project.Title = "loooooooo";
            project.Description = "loooe3rewrrewooooo";

            var user = m_database.Users.Insert(UserName: "somedude", FirstName: "Some", LastName: "Dude", APIKey: "yuiu-998",
            Email: "aaa@aaa.com", Privileges: 2, Password: "lolll");

            var response = m_repository.CreateProjectAssociatedWithKey(project, user.APIKey);
            Assert.IsTrue(response.HasSucceeded);

            Assert.AreEqual(1, m_database.Projects.All().ToList<Project>().Count);

            var projects = m_repository.GetProjectsByUseToken(user.APIKey);
            Assert.AreEqual(1, projects.Count);
        }

        private void VerifyProject(Project expected, Project actual)
        {
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Velocity, actual.Velocity);
        }


    }
}