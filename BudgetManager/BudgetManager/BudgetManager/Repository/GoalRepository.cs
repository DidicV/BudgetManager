using BudgetManager.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetManager.Repository
{
    public class GoalRepository
    {
        private readonly SQLiteAsyncConnection _connection;

        public GoalRepository(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public Task<int> AddGoal(Goal goal)
        {
            return _connection.InsertAsync(goal);
        }

        public Task<List<Goal>> GetGoals()
        {
            return _connection.Table<Goal>().ToListAsync();
        }

        public Task<int> UpdateGoal(Goal goal)
        {
            return _connection.UpdateAsync(goal);
        }

        public Task<int> DeleteGoal(Goal goal)
        {
            return _connection.DeleteAsync(goal);
        }
    }
}
