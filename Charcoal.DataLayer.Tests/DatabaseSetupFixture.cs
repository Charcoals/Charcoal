using NUnit.Framework;
using Simple.Data;

namespace Charcoal.DataLayer.Tests
{
    [SetUpFixture]
    public class DatabaseSetupFixture
    {
        

        [TearDown]
        public void ClearOutDataBase()
        {
            dynamic db = Database.OpenConnection(DatabaseHelper.GetConnectionString());
            db.Tasks.DeleteAll();
            db.Stories.DeleteAll();
            db.Projects.DeleteAll();
            db.Users.DeleteAll();
        }
    }
}