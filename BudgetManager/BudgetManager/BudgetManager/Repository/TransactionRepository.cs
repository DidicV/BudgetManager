using BudgetManager.DTO;
using BudgetManager.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetManager.Repository
{
    public class TransactionRepository
    {
        private readonly SQLiteAsyncConnection _connection;

        public TransactionRepository(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public Task<int> AddTransaction(Models.Transaction transaction)
        {
            return _connection.InsertAsync(transaction);
        }

        public Task<List<Models.Transaction>> GetTransactions()
        {
            return _connection.Table<Models.Transaction>().ToListAsync();
        }

        public Task<int> UpdateTransaction(Models.Transaction transaction)
        {
            return _connection.UpdateAsync(transaction);
        }

        public Task<int> DeleteTransactionById(int id)
        {
            return _connection.DeleteAsync<Models.Transaction>(id);
        }

        public async Task<IEnumerable<TransactionDTO>> GetTransactionsDTO()
        {
            var transactions = await _connection.Table<Models.Transaction>().ToListAsync();
            var categories = await _connection.Table<Category>().ToListAsync();
            var paymentMethods = await _connection.Table<PaymentMethod>().ToListAsync();
            var transactionTypes = await _connection.Table<TransactionType>().ToListAsync();

            var result = from t in transactions
                         join c in categories on t.CategoryId equals c.Id
                         join pm in paymentMethods on t.PaymentMethodId equals pm.Id
                         join tt in transactionTypes on t.TransactionTypeId equals tt.Id
                         select new TransactionDTO
                         {
                             Id = t.Id,
                             Amount = t.Amount,
                             Date = t.Date,
                             CreatedOn = t.CreatedOn,
                             Description = t.Description,
                             CategoryName = c.Name,
                             PaymentMethodName = pm.Name,
                             TransactionTypeName = tt.Name
                         };

            return result;
        }

        public async Task<IEnumerable<MonthlyTransactionDTO>> GetTransactionsSummary(int year, string transactionType)
        {
            var transactions = await GetTransactionsDTO();

            var groupedTransactions = new List<MonthlyTransactionDTO>();

            for (int month = 1; month <= 12; month++)
            {
                var sumForMonth = transactions
                    .Where(t => t.Date.Year == year && t.Date.Month == month && t.TransactionTypeName == transactionType)
                    .Sum(t => t.Amount);

                groupedTransactions.Add(new MonthlyTransactionDTO { Month = month, Sum = sumForMonth });
            }
            return groupedTransactions;
        }

        public async Task<bool> CheckCategoryId(int id)
        {
            var result = await GetTransactions();

            return result.Any(t => t.CategoryId == id);
        }

        public async Task<bool> CheckPaymentMethodId(int id)
        {
            var result = await GetTransactions();

            return result.Any(t => t.PaymentMethodId == id);
        }

        public async Task<bool> CheckTransactionTypeId(int id)
        {
            var result = await GetTransactions();

            return result.Any(t => t.TransactionTypeId == id);
        }

        public async Task<bool> CheckGoalId(int id)
        {
            var result = await App._transactionGoalRepository.GetTransactionGoals();

            return result.Any(g => g.TransactionId == id);
        }
    }
}