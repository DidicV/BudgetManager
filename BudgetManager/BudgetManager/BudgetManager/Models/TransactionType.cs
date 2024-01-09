using SQLite;

namespace BudgetManager.Models
{
    public class TransactionType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
