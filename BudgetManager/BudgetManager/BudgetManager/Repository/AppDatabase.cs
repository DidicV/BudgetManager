using BudgetManager.Models;
using SQLite;

namespace BudgetManager.Repository
{
    public class AppDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public AppDatabase(string connectionString)
        {
            _database = new SQLiteAsyncConnection(connectionString);
            _database.CreateTableAsync<Category>();
            _database.CreateTableAsync<TransactionType>();
            _database.CreateTableAsync<PaymentMethod>();
            _database.CreateTableAsync<Transaction>();
            _database.CreateTableAsync<Goal>();
            _database.CreateTableAsync<TransactionGoal>();

            InsertIfNotExists(new TransactionType { Name = "Incomes" });
            InsertIfNotExists(new TransactionType { Name = "Expenses" });
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return _database;
        }

        private async void InsertIfNotExists(TransactionType type)
        {
            var existingRecord = await _database.Table<TransactionType>().FirstOrDefaultAsync(x => x.Name == type.Name);

            if (existingRecord == null)
            {
                await _database.InsertAsync(type);
            }
        }
    }
}