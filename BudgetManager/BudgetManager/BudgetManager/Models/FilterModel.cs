using System;

namespace BudgetManager.Models
{
    public class FilterModel
    {
        public decimal? MinAmount { get; set; }

        public decimal? MaxAmount { get; set; }

        public string TransactionType { get; set; }

        public string Category { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now.AddYears(-5);

        public DateTime EndDate { get; set; } = DateTime.Now;

        public void ResetFilters()
        {
            MinAmount = null;
            MaxAmount = null;
            TransactionType = null;
            Category = null;
            PaymentMethod = null;
        }
    }
}
