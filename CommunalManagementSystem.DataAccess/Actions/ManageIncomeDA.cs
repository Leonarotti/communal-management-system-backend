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
    public class ManageIncomeDA : IManageIncomeDA
    {
        private readonly CommunalManagementSystemDbContext _context;

        public ManageIncomeDA(CommunalManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Income>> GetAllAsync()
        {
            var incomes = await _context.Incomes.ToListAsync();
            return incomes.Select(i => new Income
            {
                Id = i.id,
                Description = i.description,
                Amount = i.amount,
                Date = i.date,
                CreatedAt = i.created_at
            });
        }

        public async Task<Income?> GetByIdAsync(Guid id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null) return null;

            return new Income
            {
                Id = income.id,
                Description = income.description,
                Amount = income.amount,
                Date = income.date,
                CreatedAt = income.created_at
            };
        }

        public async Task<Guid> CreateAsync(Income income)
        {
            var incomeDAO = new IncomeDAO
            {
                id = Guid.NewGuid(),
                description = income.Description,
                amount = income.Amount,
                date = income.Date,
                created_at = DateTime.UtcNow
            };

            _context.Incomes.Add(incomeDAO);
            await _context.SaveChangesAsync();
            return incomeDAO.id;
        }

        public async Task<bool> UpdateAsync(Guid id, Income updatedIncome)
        {
            var incomeDAO = await _context.Incomes.FindAsync(id);
            if (incomeDAO == null) return false;

            incomeDAO.description = updatedIncome.Description;
            incomeDAO.amount = updatedIncome.Amount;
            incomeDAO.date = updatedIncome.Date;

            _context.Incomes.Update(incomeDAO);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var incomeDAO = await _context.Incomes.FindAsync(id);
            if (incomeDAO == null) return false;

            _context.Incomes.Remove(incomeDAO);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<decimal> GetTotalIncomesAsync()
        {
            return await _context.Incomes.SumAsync(i => i.amount);
        }

        public async Task<decimal> GetTotalForLast3MonthsAsync()
        {
            var fromDate = DateTime.UtcNow.AddMonths(-3);
            return await _context.Incomes
                .Where(i => i.date >= fromDate)
                .SumAsync(i => i.amount);
        }
    }
}
