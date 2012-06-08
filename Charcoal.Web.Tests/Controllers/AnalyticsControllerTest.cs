using Charcoal.Common.Providers;
using Charcoal.Web.Controllers;
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
    }
}