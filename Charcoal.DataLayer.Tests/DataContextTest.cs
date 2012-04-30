using System;
using System.Linq;
using Charcoal.Core;
using NUnit.Framework;

namespace Charcoal.DataLayer.Tests {
    [TestFixture]
    public class DataContextTest
    {
        private ISessionProvider provider = new SessionProvider();

        [Test]
        public void CanSaveSomething()
        {
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
    }
}
