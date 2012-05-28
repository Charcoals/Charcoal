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
            var client = new RestClient();
            client.BaseUrl = PivotalTrackerRestEndpoint.ENDPOINT;
            client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest();
            request.Resource = AuthenticationEndpoint;

            var response = client.Execute<AuthenticationToken>(request).Data;
            if (response != null)
            {
                return response.Guid.ToString("N");
            }
            return "";
        }
    }
}