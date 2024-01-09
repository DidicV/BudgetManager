using BudgetManager.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetManager.Repository
{
    public class PaymentMethodRepository
    {
        private readonly SQLiteAsyncConnection _connection;

        public PaymentMethodRepository(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public Task<int> AddPaymentMethod(PaymentMethod paymentMethod)
        {
            return _connection.InsertAsync(paymentMethod);
        }

        public Task<List<PaymentMethod>> GetPaymentMethods()
        {
            return _connection.Table<PaymentMethod>().ToListAsync();
        }

        public Task<int> UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            return _connection.UpdateAsync(paymentMethod);
        }

        public Task<int> DeletePaymentMethod(PaymentMethod paymentMethod)
        {
            return _connection.DeleteAsync(paymentMethod);
        }

        public Task<List<PaymentMethod>> Seach(string filter)
        {
            return _connection.Table<PaymentMethod>()
                              .Where(e => e.Name.ToLower().Contains(filter))
                              .ToListAsync();
        }
    }
}
