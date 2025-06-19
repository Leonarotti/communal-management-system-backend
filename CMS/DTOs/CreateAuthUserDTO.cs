namespace CommunalManagementSystem.API.DTOs
{
    public class CreateAuthUserDTO
    {
        public Guid PersonId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
