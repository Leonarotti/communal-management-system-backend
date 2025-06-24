using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.API.Mappers;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotaController : ControllerBase
    {
        private readonly IManageQuotaBW _manageQuotaBW;

        public QuotaController(IManageQuotaBW manageQuotaBW)
        {
            _manageQuotaBW = manageQuotaBW;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var quotas = await _manageQuotaBW.GetAllAsync();
            var quotaDTOs = QuotaMapper.QuotasToQuotaDTOs(quotas);
            return Ok(quotaDTOs);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var quota = await _manageQuotaBW.GetByIdAsync(id);
            return quota is not null
                ? Ok(QuotaMapper.QuotaToQuotaDTO(quota))
                : NotFound();
        }

        [HttpGet("person/{personId:guid}")]
        public async Task<IActionResult> GetByPerson(Guid personId)
        {
            var quotas = await _manageQuotaBW.GetByPersonAsync(personId);
            var quotaDTOs = QuotaMapper.QuotasToQuotaDTOs(quotas);
            return Ok(quotaDTOs);
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetByDate([FromQuery] int year, [FromQuery] int month)
        {
            var quotas = await _manageQuotaBW.GetByDateAsync(year, month);
            var quotaDTOs = QuotaMapper.QuotasToQuotaDTOs(quotas);
            return Ok(quotaDTOs);
        }

        [HttpGet("period")]
        public async Task<IActionResult> GetQuotaPersonByPeriod([FromQuery] Guid personId, [FromQuery] int year, [FromQuery] int month)
        {
            var quota = await _manageQuotaBW.GetByPeriodAsync(personId, year, month);
            return quota is not null
                ? Ok(QuotaMapper.QuotaToQuotaDTO(quota))
                : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQuotaDTO createQuotaDTO)
        {
            var quota = QuotaMapper.CreateQuotaDTOToQuota(createQuotaDTO);
            var id = await _manageQuotaBW.CreateAsync(quota);

            var createdQuota = await _manageQuotaBW.GetByIdAsync(id);
            if (createdQuota is null) return NotFound();

            var dto = QuotaMapper.QuotaToQuotaDTO(createdQuota);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateQuotaDTO updateQuotaDTO)
        {
            var quota = QuotaMapper.CreateQuotaDTOToQuota(updateQuotaDTO);
            var updated = await _manageQuotaBW.UpdateAsync(id, quota);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _manageQuotaBW.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpGet("total-paid")]
        public async Task<IActionResult> GetTotalQuotasPaid()
        {
            var total = await _manageQuotaBW.GetTotalQuotasPaidAsync();
            return Ok(new { Total = total });
        }

        [HttpGet("total-paid/month")]
        public async Task<IActionResult> GetTotalQuotasPaidForMonth([FromQuery] int year, [FromQuery] int month)
        {
            var total = await _manageQuotaBW.GetTotalQuotasPaidForMonthAsync(year, month);
            return Ok(new { Total = total });
        }
    }
}
