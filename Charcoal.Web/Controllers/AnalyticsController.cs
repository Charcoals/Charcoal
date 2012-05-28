using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Charcoal.Web.Controllers
{
    public class AnalyticsController : Controller
    {
        //
        // GET: /Analytics/

        public ActionResult Index(long projectId)
        {
            return View();
        }

        public ActionResult AnalyzeProject(long projectId)
        {
            return View();
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
