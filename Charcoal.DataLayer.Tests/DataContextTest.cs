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

            var obj = new Story();
            obj.Id = 4567;
            obj.ETag = Guid.NewGuid();
            obj.Description = "some text";

            context.Store(obj);
            context.SaveChanges();

            var retrieved = context.Query<Story>().Where(x => x.ETag == obj.ETag);

            Assert.IsNotNull(retrieved);
        }
    }
}
