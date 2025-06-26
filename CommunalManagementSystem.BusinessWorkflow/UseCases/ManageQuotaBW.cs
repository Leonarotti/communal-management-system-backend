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
    public class ManageQuotaBW : IManageQuotaBW
    {
        private readonly IManageQuotaDA _manageQuotaDA;

        public ManageQuotaBW(IManageQuotaDA manageQuotaDA)
        {
            _manageQuotaDA = manageQuotaDA;
        }

        public async Task<IEnumerable<Quota>> GetAllAsync()
        {
            return await _manageQuotaDA.GetAllAsync();
        }

        public async Task<Quota?> GetByIdAsync(Guid id)
        {
            return await _manageQuotaDA.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Quota>> GetByPersonAsync(Guid personId)
        {
            return await _manageQuotaDA.GetByPersonAsync(personId);
        }

        public async Task<IEnumerable<Quota>> GetByDateAsync(int year, int month)
        {
            return await _manageQuotaDA.GetByDateAsync(year, month);
        }

        public async Task<Quota?> GetByPeriodAsync(Guid personId, int year, int month)
        {
            return await _manageQuotaDA.GetByPeriodAsync(personId, year, month);
        }

        public async Task<Guid> CreateAsync(Quota quota)
        {
            // Validación
            var existing = await _manageQuotaDA.GetByPeriodAsync(quota.PersonId, quota.Year, quota.Month);
            if (existing != null)
                throw new InvalidOperationException("Ya existe una cuota para esta persona en ese periodo.");

            return await _manageQuotaDA.CreateAsync(quota);
        }

        public async Task<bool> UpdateAsync(Guid id, Quota updatedQuota)
        {
            // Validación
            var existing = await _manageQuotaDA.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Cuota no encontrada.");

            // Verificar si el periodo ya existe para otra cuota
            var periodExists = await _manageQuotaDA.GetByPeriodAsync(updatedQuota.PersonId, updatedQuota.Year, updatedQuota.Month);
            if (periodExists != null && periodExists.Id != id)
                throw new InvalidOperationException("Ya existe una cuota para esta persona en ese periodo.");

            return await _manageQuotaDA.UpdateAsync(id, updatedQuota);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _manageQuotaDA.DeleteAsync(id);
        }

        public async Task<decimal> GetTotalQuotasPaidAsync()
        {
            return await _manageQuotaDA.GetTotalQuotasPaidAsync();
        }

        public async Task<decimal> GetTotalQuotasPaidForMonthAsync(int year, int month)
        {
            return await _manageQuotaDA.GetTotalQuotasPaidForMonthAsync(year, month);
        }

        public async Task<decimal> GetTotalPaidForLast3MonthsAsync()
        {
            return await _manageQuotaDA.GetTotalPaidForLast3MonthsAsync();
        }
    }
}
