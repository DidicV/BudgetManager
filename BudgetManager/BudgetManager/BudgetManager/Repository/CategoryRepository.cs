using BudgetManager.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetManager.Repository
{
    public class CategoryRepository
    {
        private readonly SQLiteAsyncConnection _connection;

        public CategoryRepository(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public Task<int> AddCategory(Category category)
        {
            return _connection.InsertAsync(category);
        }

        public Task<List<Category>> GetCategories()
        {
            return _connection.Table<Category>().ToListAsync();
        }

        public Task<int> UpdateCategory(Category category)
        {
            return _connection.UpdateAsync(category);
        }

        public Task<int> DeleteCategory(Category category)
        {
            return _connection.DeleteAsync(category);
        }

        public Task<List<Category>> Seach(string filter)
        {
            return _connection.Table<Category>()
                              .Where(e => e.Name.ToLower().Contains(filter))
                              .ToListAsync();
        }
    }
}
