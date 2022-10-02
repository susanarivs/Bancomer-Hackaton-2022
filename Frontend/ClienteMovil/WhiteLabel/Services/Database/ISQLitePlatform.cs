using SQLite;

namespace WhiteLabel.Services.Database
{
    public interface ISQLitePlatform
    {
        SQLiteConnection GetConnection();

        SQLiteAsyncConnection GetConnectionAsync();
    }
}
