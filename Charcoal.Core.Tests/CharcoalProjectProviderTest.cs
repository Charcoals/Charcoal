using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charcoal.Common.Entities;
using Charcoal.DataLayer;
using Moq;
using NUnit.Framework;

namespace Charcoal.Core.Tests
{
    [TestFixture]
    public class CharcoalProjectProviderTest
    {
        [Test]
        public void CanRetrieveProjectsForUser()
        {
            const string token = "dude";
            var userRepo = new Mock<IProjectRepository>(MockBehavior.Strict);
            userRepo.Setup(e => e.GetProjectsByUseToken(token)).Returns(new List<Project>());

            new CharcoalProjectProvider(token, userRepo.Object).GetProjects();
            userRepo.Verify();

        }
    }
}
