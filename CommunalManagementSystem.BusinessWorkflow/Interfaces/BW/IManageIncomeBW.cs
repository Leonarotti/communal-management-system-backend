using CommunalManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.BusinessWorkflow.Interfaces.BW
{
    public interface IManageIncomeBW
    {
        Task<IEnumerable<Income>> GetAllAsync();
        Task<Income?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Income income);
        Task<bool> UpdateAsync(Guid id, Income updatedIncome);
        Task<bool> DeleteAsync(Guid id);
        Task<decimal> GetTotalIncomesAsync();
        Task<decimal> GetTotalForLast3MonthsAsync();
    }
}
