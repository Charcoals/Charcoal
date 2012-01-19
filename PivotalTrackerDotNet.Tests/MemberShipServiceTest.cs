using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PivotalTrackerDotNet.Domain;

namespace PivotalTrackerDotNet.Tests {
	[TestFixture]
	public class MemberShipServiceTest {
		private MemberShipService memberShipService = null;
		const int projectId = 456301;

		[TestFixtureSetUp]
		public void TestFixtureSetUp() {
			memberShipService = new MemberShipService(AuthenticationService.Authenticate(TestCredentials.Username, TestCredentials.Password));
		}

		[Test]
		public void CanRetrieveAllPersonsAllowedInAProject() {
			var persons = memberShipService.GetMembers(projectId);
			Assert.NotNull(persons);
			Assert.AreEqual(1, persons.Count);
		}
	}
}
