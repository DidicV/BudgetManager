using SQLite;

namespace BudgetManager.Models
{
    public class TransactionGoal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public int TransactionId { get; set; }

        public int GoalId { get; set; }
    }
}