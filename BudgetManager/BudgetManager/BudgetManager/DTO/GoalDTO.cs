namespace BudgetManager.DTO
{
    public class GoalDTO
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public int GoalId { get; set; }

        public string GoalName { get; set; }

        public decimal Amount { get; set; }
    }
}
