using BudgetManager.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetManager.Repository
{
    public class TransactionTypeRepository
    {
        private readonly SQLiteAsyncConnection _connection;

        public TransactionTypeRepository(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public Task<int> AddTransactionType(TransactionType transactionType)
        {
            return _connection.InsertAsync(transactionType);
        }

        public Task<List<TransactionType>> GetTransactionTypes()
        {
            return _connection.Table<TransactionType>().ToListAsync();
        }

        public Task<int> UpdateTransactionType(TransactionType transactionType)
        {
            return _connection.UpdateAsync(transactionType);
        }

        public Task<int> DeleteTransactionType(TransactionType transactionType)
        {
            return _connection.DeleteAsync(transactionType);
        }

        public Task<List<TransactionType>> Seach(string filter)
        {
            return _connection.Table<TransactionType>()
                              .Where(e => e.Name.ToLower().Contains(filter))
                              .ToListAsync();
        }
    }
}
