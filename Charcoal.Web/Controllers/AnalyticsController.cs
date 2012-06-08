using System;
using System.Web.Mvc;
using Charcoal.Common.Providers;

namespace Charcoal.Web.Controllers
{
    public class AnalyticsController : BaseController
    {

        readonly IStoryProvider m_storyProvider;
        IAnalyticsProvider m_analyticsProvider;
        public AnalyticsController() : this(null) { }

        public AnalyticsController(IStoryProvider storyProvider, IAnalyticsProvider analyticsProvider=null)
			: base()
        {
            this.m_storyProvider = storyProvider;
            m_analyticsProvider = analyticsProvider;
        }

        IAnalyticsProvider AnalyticsProvider
        {
            get { return m_analyticsProvider??(m_analyticsProvider=new AnalyticsProvider(m_storyProvider)); }
        }

        public ActionResult AnalyzeProject(long projectId, int velocity, string name)
        {
            var result = AnalyticsProvider.AnalyzeProject(projectId);
            result.Velocity = velocity;
            result.Name = name;
            return View(result);
        }

        public ActionResult AnalyzeLabel(long projectId, string label)
        {
            return View();
        }

        public ActionResult Projection(long projectId, string label, DateTime target)
        {
            return View();
        }

        public JsonResult ReleaseChart(long projectId, string label, DateTime target)
        {
            return null;
        }

        public JsonResult VelocityChart(long projectId, string label, DateTime target)
        {
            return null;
        }

    }
}
