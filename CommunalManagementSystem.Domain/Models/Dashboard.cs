using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.Domain.Models
{
    public class Dashboard
    {
        public int? TotalPersons { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal TotalQuotasPaid { get; set; }
    }
}
