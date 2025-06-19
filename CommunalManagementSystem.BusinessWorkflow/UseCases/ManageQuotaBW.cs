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

        public async Task<bool> UpdateStatusAsync(Guid id, string newStatus)
        {
            // Validación
            if (newStatus != "paid" && newStatus != "unpaid")
                throw new ArgumentException("El estado debe ser 'paid' o 'unpaid'.");

            return await _manageQuotaDA.UpdateStatusAsync(id, newStatus);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _manageQuotaDA.DeleteAsync(id);
        }
    }
}
