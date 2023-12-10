using BudgetManager.Models;
using SQLite;

namespace BudgetManager.Repository
{
    public class AppDatabase
    {
        private SQLiteAsyncConnection _database;

        public AppDatabase(string connectionString)
        {
            _database = new SQLiteAsyncConnection(connectionString);
            _database.CreateTableAsync<Category>();
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return _database;
        }
    }
}