using System;
using System.Collections.Generic;

namespace Charcoal.Common.Entities
{
    public class Iteration: BaseEntity
    {
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
        //public float TeamStrength { get; set; }
        public List<Story> Stories { get; set; }

        public Iteration()
        {
            Stories=new List<Story>();
        }
    }
}