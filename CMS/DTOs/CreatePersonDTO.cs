namespace CommunalManagementSystem.API.DTOs
{
    public class CreatePersonDTO
    {
        public string dni { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? phone { get; set; } 
    }
}
