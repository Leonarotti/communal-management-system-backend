using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.DataAccess.DAOs
{
    public class PersonDAO
    {
        public Guid id { get; set; }
        public string dni { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? phone { get; set; }
        public DateTime created_at { get; set; }

        // Navigation Properties
        public AuthUserDAO? AuthUser { get; set; }
        public ICollection<QuotaDAO> Quotas { get; set; } = new List<QuotaDAO>();

    }
}
