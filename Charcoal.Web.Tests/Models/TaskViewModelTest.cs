using System.Linq;
using Charcoal.Common.Entities;
using Charcoal.Web.Models;
using NUnit.Framework;

namespace Charcoal.Web.Tests.Models {
    [TestFixture]
    public class TaskViewModelTest {
        [Test]
        public void GetOwners() {
            var task = new Task { Description = "this is a task with two owners (OO/DD)" };
            var taskViewModel = new TaskViewModel(task,1, IterationType.Undefined);

            var owners = taskViewModel.GetOwners();
            Assert.AreEqual(2, owners.Count);
            Assert.AreEqual("OO", owners[0]);
            Assert.AreEqual("DD", owners[1]);

            task = new Task { Description = "this is a task with one owner (OO)" };
            taskViewModel = new TaskViewModel(task,1, IterationType.Undefined);

            var singleOwner = taskViewModel.GetOwners();
            Assert.AreEqual(1, singleOwner.Count);
            Assert.AreEqual("OO", singleOwner.First());
        }
    }
}
