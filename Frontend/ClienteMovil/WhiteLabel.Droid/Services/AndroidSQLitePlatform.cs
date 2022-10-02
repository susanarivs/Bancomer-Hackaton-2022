using System.IO;
using SQLite;
using WhiteLabel.Droid.Services;
using WhiteLabel.Services.Database;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidSQLitePlatform))]
namespace WhiteLabel.Droid.Services
{
    public class AndroidSQLitePlatform : ISQLitePlatform
    {
        private string GetPath()
        {
            const string dbName = "internaldb.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            return path;
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(GetPath());
        }

        public SQLiteAsyncConnection GetConnectionAsync()
        {
            return new SQLiteAsyncConnection(GetPath());
        }
    }
}