using Charcoal.Common.Providers;
using Charcoal.Core;
using Charcoal.PivotalTracker;

namespace Charcoal.Web.Models
{
    public class StoryProviderFactory
    {
        public IStoryProvider Create(BackingType type, string token)
        {
            if (type == BackingType.Charcoal)
                return new CharcoalStoryProvider();
            return new PTStoryProvider(token);
        }
    }
}