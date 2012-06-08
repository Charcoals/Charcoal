using Charcoal.Common.Providers;
using Charcoal.Web.Controllers;
using Charcoal.Web.Models;
using NUnit.Framework;
using Moq;

namespace Charcoal.Web.Tests.Controllers
{
    [TestFixture]
    public class AnalyticsControllerTest
    {
        [Test]
        public void CanAnalyzeProject()
        {
            var analyticsProvider = new Mock<IAnalyticsProvider>(MockBehavior.Strict);
            var projectId = 11;
            analyticsProvider.Setup(e => e.AnalyzeProject(projectId,null))
                             .Returns(new OverviewAnalysisResult());

            new AnalyticsController(null, analyticsProvider.Object).AnalyzeProject(projectId,2,"");
            analyticsProvider.VerifyAll();
        }

        [Test]
        public void CanAnalyzeStoryTag()
        {
            var analyticsProvider = new Mock<IAnalyticsProvider>(MockBehavior.Strict);
            var taggy = new TagAnalysisModel {ProjectId = 11, Tag = "lplp"};
            analyticsProvider.Setup(e => e.AnalyzeStoryTag(taggy.ProjectId, taggy.Tag, null))
                             .Returns(new OverviewAnalysisResult());

            new AnalyticsController(null, analyticsProvider.Object).AnalyzeTag(taggy);
            analyticsProvider.VerifyAll();
        }
    }
}