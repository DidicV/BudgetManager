using BudgetManager.DTO;
using BudgetManager.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetManager.Repository
{
    public class TransactionGoalRepository
    {
        private readonly SQLiteAsyncConnection _connection;

        public TransactionGoalRepository(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public Task<int> AddTransactionGoal(TransactionGoal transactionGoal)
        {
            return _connection.InsertAsync(transactionGoal);
        }

        public Task<List<TransactionGoal>> GetTransactionGoals()
        {
            return _connection.Table<TransactionGoal>().ToListAsync();
        }

        public Task<int> DeleteTransactionGoalById(int id)
        {
            return _connection.DeleteAsync<TransactionGoal>(id);
        }

        public async Task<List<GoalDTO>> GetTransactionGoalsDTO()
        {
            var goals = await _connection.Table<Goal>().ToListAsync();
            var transactions = await _connection.Table<Models.Transaction>().ToListAsync();
            var transactionGoals = await _connection.Table<TransactionGoal>().ToListAsync();

            var result = (from goal in goals
                         join transactionGoal in transactionGoals on goal.Id equals transactionGoal.GoalId
                         join transaction in transactions on transactionGoal.TransactionId equals transaction.Id
                         select new GoalDTO
                         {
                             Id = transactionGoal.Id,
                             GoalName = goal.Name,
                             Amount = transaction.Amount,
                             TransactionId = transaction.Id,
                             GoalId = goal.Id,
                         }).ToList();

            return result;
        }

        public Task<int> DeleteGoalsById(int goalId)
        {
            string query = $"DELETE FROM TransactionGoal WHERE GoalId = {goalId}";
            return _connection.ExecuteAsync(query);
        }
    }
}