using Charcoal.Core.Entities;

namespace Charcoal.Core.Services {
	public interface IAccountService
	{
		string Authenticate(string username, string password);
		OperationResponse CreateUser(User user);
	}
}
