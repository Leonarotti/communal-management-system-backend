using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.Domain.Models
{
    public class AuthUserWithPerson
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        // Person
        public Guid PersonId { get; set; }
        public string PersonName { get; set; } = null!;
        public string PersonDni { get; set; } = null!;
        public string? PersonPhone { get; set; }
    }
}
