using System.Web.Security;
using Charcoal.Common;
using Charcoal.Common.Entities;
using Charcoal.Common.Providers;

namespace Charcoal.Core
{
    public class CharcoalAccountProvider : IAccountProvider
    {
        public string Authenticate(string username, string password)
        {
            return Membership.ValidateUser(username, password)
                ? Membership.Provider.GetPassword(username, password)
                : string.Empty;
        }

        public OperationResponse CreateUser(User user)
        {
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.UserName, user.Password, user.Email, user.FirstName, user.LastName, false, null, out createStatus);
            return new OperationResponse(createStatus == MembershipCreateStatus.Success);
        }
    }
}
