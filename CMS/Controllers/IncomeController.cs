using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.API.Mappers;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using CommunalManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IManageIncomeBW _manageIncomeBW;

        public IncomeController(IManageIncomeBW manageIncomeBW)
        {
            _manageIncomeBW = manageIncomeBW;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var incomes = await _manageIncomeBW.GetAllAsync();
            return Ok(incomes);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var income = await _manageIncomeBW.GetByIdAsync(id);
            return income is not null ? Ok(income) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncomeDTO createIncomeDTO)
        {
            var income = IncomeMapper.CreateIncomeDTOToIncome(createIncomeDTO);
            var id = await _manageIncomeBW.CreateAsync(income);
            return CreatedAtAction(nameof(GetById), new { id }, income);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateIncomeDTO createIncomeDTO)
        {
            var income = IncomeMapper.CreateIncomeDTOToIncome(createIncomeDTO);
            var updated = await _manageIncomeBW.UpdateAsync(id, income);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _manageIncomeBW.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
