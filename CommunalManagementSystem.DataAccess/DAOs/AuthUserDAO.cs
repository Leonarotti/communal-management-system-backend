using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.DataAccess.DAOs
{
    public class AuthUserDAO
    {
        public Guid id { get; set; }
        public Guid person_id { get; set; }
        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
        public string role { get; set; } = null!;
        public DateTime created_at { get; set; }

        // Navigation Property
        public PersonDAO Person { get; set; } = null!;

    }
}
