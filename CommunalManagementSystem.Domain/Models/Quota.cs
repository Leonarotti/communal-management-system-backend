using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.Domain.Models
{
    public class Quota
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "unpaid";
        public DateTime CreatedAt { get; set; }
    }

}

