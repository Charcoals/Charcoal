using System;
using Charcoal.Common.Providers;

namespace Charcoal.Web.Models
{
    public class ProjectionModel
    {
        public DateTime From { get; set; }
        public DateTime TargetDate { get; set; }
        public int Iterationlength { get; set; }
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public bool IsTagAnalysis { get; set; }
    }
}