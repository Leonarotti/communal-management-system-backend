namespace CommunalManagementSystem.API.DTOs
{
    public class PersonDTO
    {
        public Guid _id { get; set; }
        public string dni { get; set; } = null!;
        public string name { get; set; } = null!;
        public string phone { get; set; }
        public DateTime created_at { get; set; }
    }
}