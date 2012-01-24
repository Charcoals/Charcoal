using System.Collections.Generic;
using PivotalTrackerDotNet.Domain;

namespace PivotalTrackerDotNet {
    public interface IActivityService {
        List<Activity> GetRecentActivity(int projectId);
        List<Activity> GetRecentActivity(int projectId, int limit);
    }
}