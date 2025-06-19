namespace CommunalManagementSystem.API.DTOs
{
    public class CreateQuotaDTO
    {
        public Guid person_id { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public decimal amount { get; set; }
    }
}
