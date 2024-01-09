using System;

namespace BudgetManager.DTO
{
    public class TransactionDTO
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Description { get; set; } = string.Empty;

        public string CategoryName { get; set; }

        public string PaymentMethodName { get; set; }

        public string TransactionTypeName { get; set; }
    }
}
