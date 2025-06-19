namespace CommunalManagementSystem.API.DTOs
{
    public class AuthUserDTO
    {
        public Guid _id { get; set; }
        public Guid person_id { get; set; }
        public string email { get; set; } = null!;
        public string role { get; set; } = null!;
        public DateTime created_at { get; set; }
    }
}
