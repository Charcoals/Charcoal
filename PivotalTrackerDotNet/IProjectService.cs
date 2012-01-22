using System;
using System.Collections.Generic;
using PivotalTrackerDotNet.Domain;
namespace PivotalTrackerDotNet {
    public interface IProjectService {
        List<Project> GetProjects();
    }
}
