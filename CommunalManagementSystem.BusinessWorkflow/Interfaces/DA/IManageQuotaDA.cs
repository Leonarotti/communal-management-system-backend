using CommunalManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.BusinessWorkflow.Interfaces.DA
{
    public interface IManageQuotaDA
    {
        Task<IEnumerable<Quota>> GetAllAsync();
        Task<Quota?> GetByIdAsync(Guid id);
        Task<IEnumerable<Quota>> GetByPersonAsync(Guid personId);
        Task<Quota?> GetByPeriodAsync(Guid personId, int year, int month);
        Task<Guid> CreateAsync(Quota quota);
        Task<bool> UpdateStatusAsync(Guid id, string newStatus);
        Task<bool> DeleteAsync(Guid id);
    }
}
