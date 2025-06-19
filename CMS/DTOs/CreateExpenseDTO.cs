namespace CommunalManagementSystem.API.DTOs
{
    public class CreateExpenseDTO
    {
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
