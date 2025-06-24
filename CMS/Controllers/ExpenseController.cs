using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.API.Mappers;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using Microsoft.AspNetCore.Mvc;

namespace CommunalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IManageExpenseBW _manageExpenseBW;

        public ExpenseController(IManageExpenseBW manageExpenseBW)
        {
            _manageExpenseBW = manageExpenseBW;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _manageExpenseBW.GetAllAsync();
            var expenseDTOs = ExpenseMapper.ExpensesToExpenseDTOs(expenses);
            return Ok(expenseDTOs);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var expense = await _manageExpenseBW.GetByIdAsync(id);
            return expense is not null
                ? Ok(ExpenseMapper.ExpenseToExpenseDTO(expense))
                : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExpenseDTO createExpenseDTO)
        {
            var expense = ExpenseMapper.CreateExpenseDTOToExpense(createExpenseDTO);
            var id = await _manageExpenseBW.CreateAsync(expense);

            var createdExpense = await _manageExpenseBW.GetByIdAsync(id);
            if (createdExpense is null) return NotFound();

            var dto = ExpenseMapper.ExpenseToExpenseDTO(createdExpense);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateExpenseDTO createExpenseDTO)
        {
            var expense = ExpenseMapper.CreateExpenseDTOToExpense(createExpenseDTO);
            var updated = await _manageExpenseBW.UpdateAsync(id, expense);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _manageExpenseBW.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotalExpenses()
        {
            var total = await _manageExpenseBW.GetTotalExpensesAsync();
            return Ok(new { Total = total });
        }
    }
}
