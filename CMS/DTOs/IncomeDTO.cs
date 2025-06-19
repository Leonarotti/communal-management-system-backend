namespace CommunalManagementSystem.API.DTOs
{
    public class IncomeDTO
    {
        public Guid _id { get; set; }
        public string description { get; set; } = null!;
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public DateTime created_at { get; set; }
    }
}
