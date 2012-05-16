using Charcoal.Common.Entities;
using Story = PivotalTrackerDotNet.Domain.Story;
using Task = PivotalTrackerDotNet.Domain.Task;
using System.Linq;

namespace PivotalTrackerDotNet
{
    public static class EntitiyConverter
    {
        public static Charcoal.Common.Entities.Task ConvertTo(this Task task)
        {
            return new Charcoal.Common.Entities.Task
                       {
                           Id = task.Id,
                           Description = task.Description,
                           IsCompleted = task.Complete,
                           StoryId = task.ParentStoryId,
                       };
        }

        public static Task ConvertTo(this Charcoal.Common.Entities.Task task, long projectId)
        {
            return new Task
                       {
                           Id = task.Id,
                           ParentStoryId = task.StoryId,
                           ProjectId = projectId,
                           Description = task.Description,
                           Complete = task.IsCompleted
                       };
        }

        public static Charcoal.Common.Entities.Story ConvertTo(this Story story, IterationType type)
        {
            return new Charcoal.Common.Entities.Story
            {
                Id = story.Id,
                Description = story.Description,
                Title = story.Name,
                IterationType = type,
                Estimate = story.Estimate,
                Status = story.CurrentState,
                StoryType = story.StoryType,
                Tasks = story.Tasks.Select(e => e.ConvertTo()).ToList(),
                ProjectId = story.ProjectId
            };
        }

        public static Story ConvertTo(this Charcoal.Common.Entities.Story story, long projectId)
        {
            return new Story
            {
                Id = story.Id,
                Description = story.Description,
                Name = story.Title,
                Estimate = story.Estimate,
                CurrentState = story.Status,
                StoryType = story.StoryType,
                Tasks = story.Tasks.Select(e => e.ConvertTo(projectId)).ToList(),
                ProjectId = story.ProjectId
            };
        }
    }
}