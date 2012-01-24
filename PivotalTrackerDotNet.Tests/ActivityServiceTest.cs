using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PivotalTrackerDotNet.Domain;

namespace PivotalTrackerDotNet.Tests {
    [TestFixture]
    public class ActivityServiceTest {
        private ActivityService activityService = null;
        const int projectId = 456301;

        [TestFixtureSetUp]
        public void TestFixtureSetUp() {
            activityService = new ActivityService(AuthenticationService.Authenticate(TestCredentials.Username, TestCredentials.Password));
        }

        [Test]
        public void CanRetrieveActivities()
        {
            var activities = activityService.GetRecentActivity(projectId);
            Assert.NotNull(activities);
            Assert.IsNotEmpty(activities);
            Assert.Greater(activities.Count,1);
        }
    }
}
