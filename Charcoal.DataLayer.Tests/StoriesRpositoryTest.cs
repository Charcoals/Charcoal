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
    public class StoriesRpositoryTest
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

            var response = m_repository.Save(story);
            Assert.IsTrue(response.HasSucceeded);

        }
    }
}
