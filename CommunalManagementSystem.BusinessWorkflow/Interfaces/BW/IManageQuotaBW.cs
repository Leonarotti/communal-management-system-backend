using CommunalManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.BusinessWorkflow.Interfaces.BW
{
    public interface IManageQuotaBW
    {
        Task<IEnumerable<Quota>> GetAllAsync();
        Task<Quota?> GetByIdAsync(Guid id);
        Task<IEnumerable<Quota>> GetByPersonAsync(Guid personId);
        Task<IEnumerable<Quota>> GetByDateAsync(int year, int month);
        Task<Quota?> GetByPeriodAsync(Guid personId, int year, int month);
        Task<Guid> CreateAsync(Quota quota);
        Task<bool> UpdateAsync(Guid id, Quota updatedQuota); // NUEVO
        Task<bool> DeleteAsync(Guid id);
        Task<decimal> GetTotalQuotasPaidAsync();
        Task<decimal> GetTotalQuotasPaidForMonthAsync(int year, int month);
        Task<decimal> GetTotalPaidForLast3MonthsAsync();
    }
}
