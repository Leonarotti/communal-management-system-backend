using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.API.Mappers
{
    public class DashboardMapper
    {
        public static DashboardDTO DashboardToDashboardDTO(Dashboard dashboard)
        {
            if (dashboard == null)
            {
                return null;
            }

            return new DashboardDTO
            {
                TotalPersons = dashboard.TotalPersons,
                TotalIncome = dashboard.TotalIncome,
                TotalExpenses = dashboard.TotalExpenses,
                CurrentBalance = dashboard.CurrentBalance,
                TotalQuotasPaid = dashboard.TotalQuotasPaid
            };
        }
    }
}
