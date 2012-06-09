using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Charcoal.Common.Providers;
using Charcoal.Web.Models;

namespace Charcoal.Web.Controllers
{
    public class AnalyticsController : BaseController
    {

        readonly IStoryProvider m_storyProvider;
        IAnalyticsProvider m_analyticsProvider;
        public AnalyticsController() : this(null) { }

        public AnalyticsController(IStoryProvider storyProvider, IAnalyticsProvider analyticsProvider = null)
            : base()
        {
            this.m_storyProvider = storyProvider;
            m_analyticsProvider = analyticsProvider;
        }

        IAnalyticsProvider AnalyticsProvider
        {
            get { return m_analyticsProvider ?? (m_analyticsProvider = new AnalyticsProvider(m_storyProvider)); }
        }

        public ActionResult AnalyzeProject(long projectId, int velocity, string name)
        {
            var result = AnalyticsProvider.AnalyzeProject(projectId);
            result.Velocity = velocity;
            result.Name = name;
            return View("AnalysisOverView", new OverviewModel(result));
        }

        public ActionResult AnalyzeTag(long projectId)
        {
            return View(new TagAnalysisModel { ProjectId = projectId });
        }

        [HttpPost]
        public ActionResult AnalyzeTag(TagAnalysisModel tagAnalysisModel)
        {
            var result = AnalyticsProvider.AnalyzeStoryTag(tagAnalysisModel.ProjectId, tagAnalysisModel.Tag);
            return View("AnalysisOverView", new OverviewModel(result));
        }

        [HttpPost]
        public ActionResult Projection(ProjectionModel model)
        {
            var overviewAnalysisResult = model.IsTagAnalysis
                                             ? AnalyticsProvider.AnalyzeStoryTag(model.ProjectId, model.Name)
                                             : AnalyticsProvider.AnalyzeProject(model.ProjectId);
            var result = AnalyticsProvider.CreateReleaseProjection(overviewAnalysisResult,
                                                                  model.TargetDate, model.Iterationlength, model.From);

            return View(result);
        }



        public JsonResult ReleaseChart(long projectId, string label, DateTime target)
        {
            return null;
        }

        public JsonResult VelocityChart(long projectId, string label, DateTime target)
        {
            return null;
        }
        //private static IterationAnalysisResult IterationAnalysisResult()
        //       {
        //           var result = new IterationAnalysisResult
        //                            {
        //                                Name = "lo",
        //                                NeededAverageVelocity = 9,
        //                                Items = new List<IterationResultItem>
        //                                            {
        //                                                new IterationResultItem
        //                                                    {
        //                                                        From = DateTime.Now,
        //                                                        BugsAdded = 3,
        //                                                        BugsFixed = 2,
        //                                                        FeaturesAccepted = 1,
        //                                                        FeaturesAdded = 3,
        //                                                        To = DateTime.Now,
        //                                                        TotalPointsCompleted = 3,
        //                                                        Velocity = 4
        //                                                    },
        //                                                new IterationResultItem
        //                                                    {
        //                                                        From = DateTime.Now,
        //                                                        BugsAdded = 3,
        //                                                        BugsFixed = 2,
        //                                                        FeaturesAccepted = 1,
        //                                                        FeaturesAdded = 3,
        //                                                        To = DateTime.Now,
        //                                                        TotalPointsCompleted = 3,
        //                                                        Velocity = 4
        //                                                    }
        //                                            }
        //                            };
        //           return result;
        //       }
    }
}
