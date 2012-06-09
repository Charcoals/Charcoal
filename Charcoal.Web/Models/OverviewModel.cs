using System.Collections.Generic;
using System.Linq;
using System.Web;
using Charcoal.Common.Providers;

namespace Charcoal.Web.Models
{
    public class OverviewModel
    {
        private readonly OverviewAnalysisResult m_result;

        public OverviewModel(OverviewAnalysisResult result)
        {
            m_result = result;
        }

        public OverviewModel()
        {

        }

        public int FeaturesCount
        {
            get { return Result.FeaturesCount; }
        }

        public string Name
        {
            get { return Result.Name; }
        }

        public int? UnplannedStoriesPoints
        {
            get { return Result.UnplannedStoriesPoints; }
        }

        public int UnestimatedStoriesCount
        {
            get { return Result.UnestimatedStoriesCount; }
        }

        public int? Velocity
        {
            get { return Result.Velocity; }
        }

        public int TotalPointsLeft
        {
            get { return Result.TotalPointsLeft; }
        }

        public int TotalPointsCompleted
        {
            get { return Result.TotalPointsCompleted; }
        }

        public int TotalBugsCount
        {
            get { return Result.TotalBugsCount; }
        }

        public int RemainingBugsCount
        {
            get { return Result.RemainingBugsCount; }
        }

        public OverviewAnalysisResult Result
        {
            get { return m_result; }
        }

        public long ProjectId
        {
            get { return Result.ProjectId; }
        }

        public bool IsTagAnalysis
        {
            get { return Result.IsTagAnalysis; }
        }
    }
}