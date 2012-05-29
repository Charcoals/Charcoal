using Charcoal.Common.Providers;
using Charcoal.Core;
using Charcoal.PivotalTracker;

namespace Charcoal.Web.Models
{
    public class ProjectProviderFactory
    {
        public IProjectProvider Create(AuthenticationType type, string token)
        {
            if (type == AuthenticationType.Charcoal)
                return new CharcoalProjectProvider(token);
            return new PTProjectProvider(token);
        }
    }
}