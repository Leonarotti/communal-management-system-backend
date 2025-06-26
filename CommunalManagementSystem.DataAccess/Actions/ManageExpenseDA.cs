using CommunalManagementSystem.BusinessWorkflow.Interfaces.DA;
using CommunalManagementSystem.DataAccess.Context;
using CommunalManagementSystem.DataAccess.DAOs;
using CommunalManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunalManagementSystem.DataAccess.Actions
{
    public class ManageExpenseDA : IManageExpenseDA
    {
        private readonly CommunalManagementSystemDbContext _context;

        public ManageExpenseDA(CommunalManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetAllAsync()
        {
            var expenses = await _context.Expenses.ToListAsync();
            return expenses.Select(e => new Expense
            {
                Id = e.id,
                Description = e.description,
                Amount = e.amount,
                Date = e.date,
                CreatedAt = e.created_at
            });
        }

        public async Task<Expense?> GetByIdAsync(Guid id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return null;

            return new Expense
            {
                Id = expense.id,
                Description = expense.description,
                Amount = expense.amount,
                Date = expense.date,
                CreatedAt = expense.created_at
            };
        }

        public async Task<Guid> CreateAsync(Expense expense)
        {
            var expenseDAO = new ExpenseDAO
            {
                id = Guid.NewGuid(),
                description = expense.Description,
                amount = expense.Amount,
                date = expense.Date,
                created_at = DateTime.UtcNow
            };

            _context.Expenses.Add(expenseDAO);
            await _context.SaveChangesAsync();
            return expenseDAO.id;
        }

        public async Task<bool> UpdateAsync(Guid id, Expense updatedExpense)
        {
            var expenseDAO = await _context.Expenses.FindAsync(id);
            if (expenseDAO == null) return false;

            expenseDAO.description = updatedExpense.Description;
            expenseDAO.amount = updatedExpense.Amount;
            expenseDAO.date = updatedExpense.Date;

            _context.Expenses.Update(expenseDAO);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var expenseDAO = await _context.Expenses.FindAsync(id);
            if (expenseDAO == null) return false;

            _context.Expenses.Remove(expenseDAO);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<decimal> GetTotalExpensesAsync()
        {
            return await _context.Expenses.SumAsync(e => e.amount);
        }

        public async Task<decimal> GetTotalForLast3MonthsAsync()
        {
            var fromDate = DateTime.UtcNow.AddMonths(-3);
            return await _context.Expenses
                .Where(e => e.date >= fromDate)
                .SumAsync(e => e.amount);
        }

    }
}
