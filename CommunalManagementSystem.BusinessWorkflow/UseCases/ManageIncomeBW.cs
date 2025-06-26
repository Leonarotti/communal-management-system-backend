using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.DA;
using CommunalManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.BusinessWorkflow.UseCases
{
    public class ManageIncomeBW : IManageIncomeBW
    {
        private readonly IManageIncomeDA _manageIncomeDA;

        public ManageIncomeBW(IManageIncomeDA manageIncomeDA)
        {
            _manageIncomeDA = manageIncomeDA;
        }

        public async Task<Guid> CreateAsync(Income income)
        {
            if (income.Amount <= 0)
                throw new ArgumentException("El monto del ingreso debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(income.Description))
                throw new ArgumentException("La descripción del ingreso no puede estar vacía.");

            income.Id = Guid.NewGuid();
            income.CreatedAt = DateTime.UtcNow;

            return await _manageIncomeDA.CreateAsync(income);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingIncome = await _manageIncomeDA.GetByIdAsync(id);
            if (existingIncome == null)
                throw new InvalidOperationException("No se encontró el ingreso a eliminar.");

            return await _manageIncomeDA.DeleteAsync(id);
        }

        public Task<IEnumerable<Income>> GetAllAsync()
        {
            return _manageIncomeDA.GetAllAsync();
        }

        public async Task<Income?> GetByIdAsync(Guid id)
        {
            var income = await _manageIncomeDA.GetByIdAsync(id);
            if (income == null)
                return null;

            return income;
        }

        public async Task<bool> UpdateAsync(Guid id, Income updatedIncome)
        {
            var existingIncome = await _manageIncomeDA.GetByIdAsync(id);
            if (existingIncome == null)
                throw new InvalidOperationException("No se encontró el ingreso para actualizar.");

            if (updatedIncome.Amount <= 0)
                throw new ArgumentException("El monto debe ser mayor que cero.");

            // Mantener la fecha original de creación
            updatedIncome.CreatedAt = existingIncome.CreatedAt;

            return await _manageIncomeDA.UpdateAsync(id, updatedIncome);
        }

        public async Task<decimal> GetTotalIncomesAsync()
        {
            return await _manageIncomeDA.GetTotalIncomesAsync();
        }

        public async Task<decimal> GetTotalForLast3MonthsAsync()
        {
            return await _manageIncomeDA.GetTotalForLast3MonthsAsync();
        }

    }
}
