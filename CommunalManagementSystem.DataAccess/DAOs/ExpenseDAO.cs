using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.DataAccess.DAOs
{
    public class ExpenseDAO
    {
        public Guid id { get; set; }
        public string description { get; set; } = null!;
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public DateTime created_at { get; set; }
    }
}
