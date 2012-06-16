using System.ComponentModel.DataAnnotations;
using Charcoal.Common.Entities;

namespace Charcoal.Web.Models
{
    public class ProjectModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Project Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Project Description")]
        public string Description { get; set; }

        public int Velocity { get; set; }
        public long Id { get; set; }

        public Project ConvertToProject()
        {
            return new Project
                       {
                           Id = Id,
                           Velocity = Velocity,
                           Description = Description,
                           Title = Title
                       };
        }
    }
}