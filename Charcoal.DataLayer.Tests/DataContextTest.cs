using System;
using System.Collections.Generic;
using System.Linq;
using Charcoal.Core;
using NUnit.Framework;

namespace Charcoal.DataLayer.Tests {
    [TestFixture]
    public class DataContextTest
    {
        private ISessionProvider provider = new SessionProvider();

        [Test]
        public void CanSaveSomething() {
            var session = provider.Session();

            var context = new DataContext(session);

            var id = Guid.NewGuid();

            var obj = new Story();
            obj.SetId(id);
            obj.ETag = Guid.NewGuid();
            obj.Description = "some text";

            var expectedKey = "stories/" + id;

            Assert.AreEqual(expectedKey, obj.Id);
            context.Store(obj);
            context.SaveChanges();

            var queried = context.Query<Story>().Where(x => x.ETag == obj.ETag);
            Assert.NotNull(queried);

            var loaded = context.Load<Story>(expectedKey);
            Assert.NotNull(loaded);
        }

        [Test]
        public void CanQueryForChildObject() {
            //this kinda sucks
            var session = provider.Session();

            var context = new DataContext(session);

            var id = Guid.NewGuid();
            var taskid = Guid.NewGuid();

            var expectedKey = "stories/" + id;
            var expectedTaskId = "tasks/" + taskid;

            var taskDescription = "asdfasdfasdf";

            var story = new Story();
            story.SetId(id);
            story.ETag = Guid.NewGuid();
            story.Description = "some text";
            story.Tasks = new List<Task> { new Task { Description = taskDescription, ETag = Guid.NewGuid() }};
            story.Tasks.First().SetId(taskid);

            Assert.AreEqual(expectedKey, story.Id);
            Assert.AreEqual(expectedTaskId, story.Tasks.First().Id);

            context.Store(story);
            context.SaveChanges();

            var queried = context.Query<Story>()
                .Where(st => st.Tasks.Any(t => t.Id == expectedTaskId))
                .ToList() //SelectMany not in raven's linq provider
                .SelectMany(st => st.Tasks)
                .Where(t => t.Id == expectedTaskId)
                .ToList();

            Assert.AreEqual(1, queried.Count());
            Assert.AreEqual(taskDescription, queried.First().Description);

        }
    }
}
