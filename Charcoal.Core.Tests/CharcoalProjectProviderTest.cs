using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charcoal.Common;
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
            var projectRepo = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepo.Setup(e => e.GetProjectsByUseToken(token)).Returns(new List<dynamic>());

            new CharcoalProjectProvider(token, projectRepo.Object).GetProjects();
            projectRepo.Verify();
        }

        [Test]
        public void CanCreateProject()
        {
            var project = new Project {Description = "re"};
            var projectRepo = new Mock<IProjectRepository>(MockBehavior.Strict);
            var apiToken = "key";
            projectRepo.Setup(e => e.CreateProjectAssociatedWithKey(project, apiToken)).Returns(new DatabaseOperationResponse(true));

            new CharcoalProjectProvider(apiToken, projectRepo.Object).CreateProject(project);
            projectRepo.Verify();
        }

    }
}
