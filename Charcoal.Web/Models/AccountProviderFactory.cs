using Charcoal.Common.Providers;
using Charcoal.Core;
using Charcoal.PivotalTracker;

namespace Charcoal.Web.Models
{
    public class AccountProviderFactory
    {
        public IAccountProvider Create(AuthenticationType type)
        {
            if(type== AuthenticationType.Charcoal)
                return new CharcoalAccountProvider();
            return new PTAuthenticationProvider();
        }
    }
}