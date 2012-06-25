using System;
using Charcoal.Common.Entities;
using PivotalTrackerDotNet.Domain;
using System.Linq;
using Iteration = PivotalTrackerDotNet.Domain.Iteration;
using Story = PivotalTrackerDotNet.Domain.Story;
using StoryStatus = PivotalTrackerDotNet.Domain.StoryStatus;
using StoryType = PivotalTrackerDotNet.Domain.StoryType;
using Task = PivotalTrackerDotNet.Domain.Task;

namespace Charcoal.PivotalTracker
{
    public static class Extensions
    {
        public static Common.Entities.Iteration ConvertTo(this Iteration iteration, IterationType type)
        {
            var convertedIteration = new Common.Entities.Iteration
                                {
                                    Id = iteration.Id,
                                    Finish = iteration.FinishDate,
                                    Start = iteration.StartDate
                                };

            convertedIteration.Stories.AddRange(iteration.Stories.Select(e=> e.ConvertTo(type)));
            return convertedIteration;
        }

        public static Charcoal.Common.Entities.Task ConvertTo(this Task task)
        {
            return new Charcoal.Common.Entities.Task
                       {
                           Id = task.Id,
                           Description = task.Description,
                           IsCompleted = task.Complete,
                           StoryId = task.ParentStoryId,
                           Position = task.Position
                       };
        }

        public static Task ConvertTo(this Charcoal.Common.Entities.Task task, long projectId)
        {
            return new Task
                       {
                           Id = (int)task.Id,
                           ParentStoryId = (int)task.StoryId,
                           ProjectId = (int)projectId,
                           Description = task.Description,
                           Complete = task.IsCompleted,
                           Position = task.Position
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
                Status = ConvertTo(story.CurrentState),
                StoryType = ConvertTo(story.StoryType),
                Tasks = story.Tasks.Select(e => e.ConvertTo()).ToList(),
                ProjectId = story.ProjectId,
                AcceptedOn = story.AcceptedOn,
                CreatedOn = story.CreatedOn.Value
            };
        }

        public static Story ConvertTo(this Charcoal.Common.Entities.Story story, long projectId)
        {
            return new Story
            {
                Id = (int)story.Id,
                Description = story.Description,
                Name = story.Title,
                Estimate = story.Estimate ?? 0,
                CurrentState = ConvertTo(story.Status),
                StoryType = ConvertTo(story.StoryType),
                Tasks = story.Tasks.Select(e => e.ConvertTo(projectId)).ToList(),
                ProjectId = (int)story.ProjectId,
            };
        }

        public static AuthenticationToken ParseToken(this string token)
        {
            return new AuthenticationToken { Guid = Guid.Parse(token) };
        }

        private static StoryStatus ConvertTo(Common.Entities.StoryStatus val)
        {
            switch (val)
            {
                case Common.Entities.StoryStatus.UnScheduled:
                    return StoryStatus.UnScheduled;
                case Common.Entities.StoryStatus.UnStarted:
                    return StoryStatus.UnStarted;
                case Common.Entities.StoryStatus.Started:
                    return StoryStatus.Started;
                case Common.Entities.StoryStatus.Finished:
                    return StoryStatus.Finished;
                case Common.Entities.StoryStatus.Delivered:
                    return StoryStatus.Delivered;
                case Common.Entities.StoryStatus.Accepted:
                    return StoryStatus.Accepted;
                case Common.Entities.StoryStatus.Rejected:
                    return StoryStatus.Rejected;
                default:
                    throw new ArgumentOutOfRangeException("val");
            }
        }

        private static Common.Entities.StoryStatus ConvertTo(StoryStatus val)
        {
            switch (val)
            {
                case StoryStatus.UnScheduled:
                    return Common.Entities.StoryStatus.UnScheduled;
                case StoryStatus.UnStarted:
                    return Common.Entities.StoryStatus.UnStarted;
                case StoryStatus.Started:
                    return Common.Entities.StoryStatus.Started;
                case StoryStatus.Finished:
                    return Common.Entities.StoryStatus.Finished;
                case StoryStatus.Delivered:
                    return Common.Entities.StoryStatus.Delivered;
                case StoryStatus.Accepted:
                    return Common.Entities.StoryStatus.Accepted;
                case StoryStatus.Rejected:
                    return Common.Entities.StoryStatus.Rejected;
                default:
                    throw new ArgumentOutOfRangeException("val");
            }
        }

        private static StoryType ConvertTo(this Common.Entities.StoryType val)
        {
            switch (val)
            {
                case Common.Entities.StoryType.Feature:
                    return StoryType.Feature;
                case Common.Entities.StoryType.Bug:
                    return StoryType.Bug;
                case Common.Entities.StoryType.Chore:
                    return StoryType.Chore;
                default:
                    throw new ArgumentOutOfRangeException("val");
            }
        }

        private static Common.Entities.StoryType ConvertTo(this StoryType val)
        {
            switch (val)
            {
                case StoryType.Bug:
                    return Common.Entities.StoryType.Bug;
                case StoryType.Chore:
                    return Common.Entities.StoryType.Chore;
                case StoryType.Feature:
                    return Common.Entities.StoryType.Feature;
                default:
                    throw new ArgumentOutOfRangeException("val");
            }
        }
    }
}