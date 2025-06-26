using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.BusinessWorkflow.Interfaces.DA
{
    public interface IManageExpenseDA
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
