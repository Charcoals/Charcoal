using NUnit.Framework;
using Simple.Data;

namespace Charcoal.DataLayer.Tests
{
    [SetUpFixture]
    public class DatabaseSetupFixture
    {
        [SetUp]
        public void Init()
        {
            dynamic db = Database.OpenConnection(DatabaseHelper.GetConnectionString());
            var updates= db.Users.Insert(UserName: "somedude", FirstName: "Some", LastName: "Dude", APIKey: "yuiu-998",
                                         Email: "aaa@aaa.com", Privileges: 2);

        }

        [TearDown]
        public void ClearOutDataBase()
        {
            dynamic db = Database.OpenConnection(DatabaseHelper.GetConnectionString());
            db.Tasks.DeleteAll();
            db.Stories.DeleteAll();
            db.Users.DeleteAll();
        }
    }
}