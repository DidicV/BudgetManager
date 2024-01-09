using SQLite;

namespace BudgetManager.Models
{
    public class PaymentMethod
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
