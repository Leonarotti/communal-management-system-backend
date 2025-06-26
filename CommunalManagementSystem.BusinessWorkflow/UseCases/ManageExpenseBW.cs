using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.DA;
using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.BusinessWorkflow.UseCases
{
    public class ManageExpenseBW : IManageExpenseBW
    {
        private readonly IManageExpenseDA _manageExpenseDA;

        public ManageExpenseBW(IManageExpenseDA manageExpenseDA)
        {
            _manageExpenseDA = manageExpenseDA;
        }

        public async Task<Guid> CreateAsync(Expense expense)
        {
            if (expense.Amount <= 0)
                throw new ArgumentException("El monto del gasto debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(expense.Description))
                throw new ArgumentException("La descripción del gasto no puede estar vacía.");

            expense.Id = Guid.NewGuid();
            expense.CreatedAt = DateTime.UtcNow;

            return await _manageExpenseDA.CreateAsync(expense);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingExpense = await _manageExpenseDA.GetByIdAsync(id);
            if (existingExpense == null)
                throw new InvalidOperationException("No se encontró el gasto a eliminar.");

            return await _manageExpenseDA.DeleteAsync(id);
        }

        public Task<IEnumerable<Expense>> GetAllAsync()
        {
            return _manageExpenseDA.GetAllAsync();
        }

        public async Task<Expense?> GetByIdAsync(Guid id)
        {
            return await _manageExpenseDA.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Guid id, Expense updatedExpense)
        {
            var existingExpense = await _manageExpenseDA.GetByIdAsync(id);
            if (existingExpense == null)
                throw new InvalidOperationException("No se encontró el gasto para actualizar.");

            if (updatedExpense.Amount <= 0)
                throw new ArgumentException("El monto debe ser mayor que cero.");

            updatedExpense.CreatedAt = existingExpense.CreatedAt;

            return await _manageExpenseDA.UpdateAsync(id, updatedExpense);
        }

        public async Task<decimal> GetTotalExpensesAsync()
        {
            return await _manageExpenseDA.GetTotalExpensesAsync();
        }

        public async Task<decimal> GetTotalForLast3MonthsAsync()
        {
            return await _manageExpenseDA.GetTotalForLast3MonthsAsync();
        }

    }
}
