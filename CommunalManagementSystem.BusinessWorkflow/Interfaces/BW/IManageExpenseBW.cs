using CommunalManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.BusinessWorkflow.Interfaces.BW
{
    public interface IManageExpenseBW
    {
        Task<IEnumerable<Expense>> GetAllAsync();
        Task<Expense?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Expense expense);
        Task<bool> UpdateAsync(Guid id, Expense updatedExpense);
        Task<bool> DeleteAsync(Guid id);
        Task<decimal> GetTotalExpensesAsync();
        Task<decimal> GetTotalForLast3MonthsAsync();
    }
}
