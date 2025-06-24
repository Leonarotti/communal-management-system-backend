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
            var incomeDTOs = IncomeMapper.IncomesToIncomeDTOs(incomes);
            return Ok(incomeDTOs);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var income = await _manageIncomeBW.GetByIdAsync(id);
            return income is not null
                ? Ok(IncomeMapper.IncomeToIncomeDTO(income))
                : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncomeDTO createIncomeDTO)
        {
            var income = IncomeMapper.CreateIncomeDTOToIncome(createIncomeDTO);
            var id = await _manageIncomeBW.CreateAsync(income);

            // Buscarlo para devolver la versión con todos los datos completos
            var createdIncome = await _manageIncomeBW.GetByIdAsync(id);
            if (createdIncome is null) return NotFound();

            var dto = IncomeMapper.IncomeToIncomeDTO(createdIncome);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
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

        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var total = await _manageIncomeBW.GetTotalIncomesAsync();
            return Ok(new { Total = total });
        }
    }
}
