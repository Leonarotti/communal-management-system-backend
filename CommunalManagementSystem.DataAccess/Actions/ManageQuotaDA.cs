using CommunalManagementSystem.BusinessWorkflow.Interfaces.DA;
using CommunalManagementSystem.DataAccess.Context;
using CommunalManagementSystem.DataAccess.DAOs;
using CommunalManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.DataAccess.Actions
{
    public class ManageQuotaDA : IManageQuotaDA
    {
        private readonly CommunalManagementSystemDbContext _context;

        public ManageQuotaDA(CommunalManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Quota>> GetAllAsync()
        {
            var quotas = await _context.Quotas.ToListAsync();
            return quotas.Select(q => QuotaDAOToQuota(q));
        }

        public async Task<Quota?> GetByIdAsync(Guid id)
        {
            var quota = await _context.Quotas.FindAsync(id);
            return quota == null ? null : QuotaDAOToQuota(quota);
        }

        public async Task<IEnumerable<Quota>> GetByPersonAsync(Guid personId)
        {
            var quotas = await _context.Quotas
                .Where(q => q.person_id == personId)
                .OrderByDescending(q => q.year)
                .ThenByDescending(q => q.month)
                .ToListAsync();

            return quotas.Select(q => QuotaDAOToQuota(q));
        }

        public async Task<IEnumerable<Quota>> GetByDateAsync(int year, int month)
        {
            var quotas = await _context.Quotas
                .Where(q => q.year == year && q.month == month)
                .ToListAsync();

            return quotas.Select(q => new Quota
            {
                Id = q.id,
                PersonId = q.person_id,
                Year = q.year,
                Month = q.month,
                Amount = q.amount,
                CreatedAt = q.created_at
            });
        }

        public async Task<Quota?> GetByPeriodAsync(Guid personId, int year, int month)
        {
            var quota = await _context.Quotas
                .FirstOrDefaultAsync(q => q.person_id == personId && q.year == year && q.month == month);

            return quota == null ? null : QuotaDAOToQuota(quota);
        }

        public async Task<Guid> CreateAsync(Quota quota)
        {
            var dao = QuotaToQuotaDAO(quota);
            dao.id = Guid.NewGuid();
            dao.created_at = DateTime.UtcNow;

            _context.Quotas.Add(dao);
            await _context.SaveChangesAsync();
            return dao.id;
        }

        public async Task<bool> UpdateAsync(Guid id, Quota updatedQuota)
        {
            var existing = await _context.Quotas.FindAsync(id);
            if (existing is null) return false;

            existing.year = updatedQuota.Year;
            existing.month = updatedQuota.Month;
            existing.amount = updatedQuota.Amount;
            // No actualizamos created_at para mantener la integridad de creación

            _context.Quotas.Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var quota = await _context.Quotas.FindAsync(id);
            if (quota == null)
                return false;

            _context.Quotas.Remove(quota);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<decimal> GetTotalQuotasPaidAsync()
        {
            return await _context.Quotas.SumAsync(q => q.amount);
        }

        public async Task<decimal> GetTotalQuotasPaidForMonthAsync(int year, int month)
        {
            return await _context.Quotas
                .Where(q => q.year == year && q.month == month)
                .SumAsync(q => q.amount);
        }

        public async Task<decimal> GetTotalPaidForLast3MonthsAsync()
        {
            var fromDate = DateTime.UtcNow.AddMonths(-3);
            return await _context.Quotas
                .Where(q => q.month >= fromDate.Month && q.year >= fromDate.Year)
                .SumAsync(q => q.amount);
        }

        // Map from DAO to Domain
        private Quota QuotaDAOToQuota(QuotaDAO quotaDAO) => new Quota
        {
            Id = quotaDAO.id,
            PersonId = quotaDAO.person_id,
            Year = quotaDAO.year,
            Month = quotaDAO.month,
            Amount = quotaDAO.amount,
            CreatedAt = quotaDAO.created_at
        };

        // Map from Domain to DAO
        private QuotaDAO QuotaToQuotaDAO(Quota quota) => new QuotaDAO
        {
            id = quota.Id,
            person_id = quota.PersonId,
            year = quota.Year,
            month = quota.Month,
            amount = quota.Amount,
            created_at = quota.CreatedAt
        };
    }
}
