using System;
using Charcoal.Common;
using Charcoal.Common.Entities;
using Charcoal.Common.Providers;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;
using RestSharp;

namespace Charcoal.PivotalTracker
{
    public class PTAuthenticationProvider : IAccountProvider
    {
        private const string AuthenticationEndpoint = "tokens/active";

        public OperationResponse CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public string Authenticate(string username, string password)
        {
            var response = AuthenticationService.Authenticate(username, password);
            return response != null ? response.Guid.ToString("N") : "";
        }
    }
}