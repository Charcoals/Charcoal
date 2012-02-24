using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PivotalTrackerDotNet.Domain;
using PivotalExtension.TaskManager.Models;

namespace PivotalExtension.TaskManager.Tests.Models {
    [TestFixture]
    public class TaskViewModelTest {
        [Test]
        public void GetOwners() {
            var task = new Task { Description = "this is a task with two owners (OO/DD)" };
            var taskViewModel = new TaskViewModel(task);

            var owners = taskViewModel.GetOwners();
            Assert.AreEqual(2, owners.Count);
            Assert.AreEqual("OO", owners[0]);
            Assert.AreEqual("DD", owners[1]);

            task = new Task { Description = "this is a task with one owner (OO)" };
            taskViewModel = new TaskViewModel(task);

            var singleOwner = taskViewModel.GetOwners();
            Assert.AreEqual(1, singleOwner.Count);
            Assert.AreEqual("OO", singleOwner.First());
        }
    }
}
