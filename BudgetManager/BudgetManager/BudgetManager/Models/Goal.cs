using SQLite;

namespace BudgetManager.Models
{
    public class Goal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal CurrentAmount { get; set; } 

        public decimal TargetAmount { get; set; }

        public double Progress { get; set; }
    }
}
