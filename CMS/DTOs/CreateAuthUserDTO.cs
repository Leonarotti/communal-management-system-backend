﻿namespace CommunalManagementSystem.API.DTOs
{
    public class CreateAuthUserDTO
    {
        public Guid person_id { get; set; }
        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
        public string role { get; set; } = null!;
    }
}
