namespace CommunalManagementSystem.API.DTOs
{
    public class QuotaDTO
    {
        public Guid _id { get; set; }
        public Guid person_id { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public decimal amount { get; set; }
        public DateTime created_at { get; set; }
    }
}
