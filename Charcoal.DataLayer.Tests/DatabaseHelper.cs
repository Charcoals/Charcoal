using System.IO;

namespace Charcoal.DataLayer.Tests
{
    public class DatabaseHelper
    {
        const string ConnectionString =
            @"Data Source=.\SQLEXPRESS;AttachDbFilename={0}\TestDatabase.mdf;Integrated Security=True;User Instance=True";

        public static string GetConnectionString()
        {
            return string.Format(ConnectionString,
                                 Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName));
        }

    }
}