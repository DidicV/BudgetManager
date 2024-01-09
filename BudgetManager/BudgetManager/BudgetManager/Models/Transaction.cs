using SQLite;
using System;

namespace BudgetManager.Models
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public int PaymentMethodId { get; set; }

        public int TransactionTypeId { get; set; }
    }
}
