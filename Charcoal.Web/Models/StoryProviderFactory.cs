using Charcoal.Common.Providers;
using Charcoal.Core;
using Charcoal.PivotalTracker;

namespace Charcoal.Web.Models
{
    public class StoryProviderFactory
    {
        public IStoryProvider Create(AuthenticationType type, string token)
        {
            if (type == AuthenticationType.Charcoal)
                return new CharcoalStoryProvider();
            return new PTStoryProvider(token);
        }
    }
}