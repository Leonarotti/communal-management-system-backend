namespace CommunalManagementSystem.API.DTOs
{
    public class DashboardDTO
    {
        public int? TotalPersons { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal TotalQuotasPaid { get; set; }
    }
}
