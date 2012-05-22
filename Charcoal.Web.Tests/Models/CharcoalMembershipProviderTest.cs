using Charcoal.Common.Entities;
using Charcoal.DataLayer;
using NUnit.Framework;
using Charcoal.Web.Models;
using Moq;
using System.Web.Security;

namespace Charcoal.Web.Tests.Models
{
    [TestFixture]
    public class CharcoalMembershipProviderTest
    {
        [Test]
        public void CanCreateUser()
        {
            var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
            var newUser = new User
            {
                UserName = "Name",
                Password = "Pass",
                Email = "Email",
                FirstName = "You",
                LastName = "Me"
            };

            userRepo.Setup(e => e.FindByEmail(newUser.Email)).Returns((User)null);
            userRepo.Setup(e => 
                e.Save(It.Is<User>(user=> user.Email.Equals(newUser.Email) 
                                       && user.UserName.Equals(newUser.UserName)
                                       && user.LastName.Equals(newUser.LastName)
                                       && user.FirstName.Equals(newUser.FirstName)
                                       && user.Password.Equals(newUser.Password))))
                .Returns(new DatabaseOperationResponse(true));

            MembershipCreateStatus status;

            new CharcoalMembershipProvider(userRepo.Object)
                .CreateUser(newUser.UserName,
                newUser.Password, newUser.Email,
                newUser.FirstName, newUser.LastName,
                false, null, out status);
            Assert.AreEqual(MembershipCreateStatus.Success, status);
            userRepo.Verify();
        }

        [Test]
        public void CanValidateUserExistence()
        {
            var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
            const string userName = "dude";
            const string password = "pass";
            userRepo.Setup(e => e.IsValid(userName, password)).Returns(true);

            var isValid = new CharcoalMembershipProvider(userRepo.Object)
                .ValidateUser(userName,password);
            Assert.IsTrue(isValid);
            userRepo.Verify();
        }

        [Test]
        public void CanGetUserByEmail()
        {
            const string myMail = "dude@email.com";
            var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
            const string userName = "dude";
            userRepo.Setup(e => e.FindByEmail(myMail)).Returns(new User { UserName = userName });

            var retirevedUserName = new CharcoalMembershipProvider(userRepo.Object).GetUserNameByEmail(myMail);
            Assert.AreEqual(userName, retirevedUserName);
            userRepo.Verify();
        }

        [Test]
        public void CanGetUserAPI()
        {
            var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
            var user = new User {UserName = "dude", Password = "lol",APIKey = "jawn"};
            userRepo.Setup(e => e.GetAPIKey(user.UserName, user.Password)).Returns(user.APIKey);

            var apiKey = new CharcoalMembershipProvider(userRepo.Object).GetPassword(user.UserName,user.Password);
            Assert.AreEqual(user.APIKey, apiKey);
            userRepo.Verify();
        }
    }
}
