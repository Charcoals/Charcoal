using Charcoal.Common.Entities;

namespace Charcoal.Common.Providers {
	public interface IAccountProvider
	{
		string Authenticate(string username, string password);
		OperationResponse CreateUser(User user);
	}
}
