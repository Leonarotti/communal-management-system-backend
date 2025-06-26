using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.BusinessWorkflow.Interfaces.BW
{
    public interface IDashboardBW
    {
        Task<Dashboard> GetDashboardMetricsAsync();
        Task<Dashboard> GetQuarterSummary();
    }
}
