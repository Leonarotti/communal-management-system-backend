using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.DataAccess.DAOs
{
    public class QuotaDAO
    {
        public Guid id { get; set; }
        public Guid person_id { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public decimal amount { get; set; }
        public string status { get; set; } = "unpaid";
        public DateTime created_at { get; set; }

        // Navigation Property
        public PersonDAO Person { get; set; } = null!;
    }
}
