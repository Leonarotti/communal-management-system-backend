namespace CommunalManagementSystem.API.DTOs
{
    public class AuthUserWithPersonDTO
    {
        public Guid _id { get; set; }
        public string email { get; set; } = null!;
        public string role { get; set; } = null!;
        public DateTime created_at { get; set; }

        // Person
        public Guid person_id { get; set; }
        public string person_name { get; set; } = null!;
        public string person_dni { get; set; } = null!;
        public string? person_phone { get; set; }
    }
}
