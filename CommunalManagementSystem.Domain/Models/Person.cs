using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.Domain.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Dni { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
