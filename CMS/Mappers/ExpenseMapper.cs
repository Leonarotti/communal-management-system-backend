using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.API.Mappers
{
    public class ExpenseMapper
    {
        public static ExpenseDTO ExpenseToExpenseDTO(Expense expense)
        {
            return new ExpenseDTO
            {
                _id = expense.Id,
                description = expense.Description,
                amount = expense.Amount,
                date = expense.Date,
                created_at = expense.CreatedAt
            };
        }

        public static Expense ExpenseDTOToExpense(ExpenseDTO expenseDTO)
        {
            return new Expense
            {
                Id = expenseDTO._id,
                Description = expenseDTO.description,
                Amount = expenseDTO.amount,
                Date = expenseDTO.date,
                CreatedAt = expenseDTO.created_at
            };
        }

        public static IEnumerable<ExpenseDTO> ExpensesToExpenseDTOs(IEnumerable<Expense> expenses)
        {
            return expenses.Select(ExpenseToExpenseDTO);
        }

        public static Expense CreateExpenseDTOToExpense(CreateExpenseDTO createExpenseDTO)
        {
            return new Expense
            {
                Description = createExpenseDTO.Description,
                Amount = createExpenseDTO.Amount,
                Date = createExpenseDTO.Date,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
