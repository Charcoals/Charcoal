using System;
namespace PivotalTrackerDotNet {
    public interface IMembershipService {
        System.Collections.Generic.List<PivotalTrackerDotNet.Domain.Person> GetMembers(int projectId);
    }
}
