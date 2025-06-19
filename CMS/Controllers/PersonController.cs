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
    public class PersonController : ControllerBase
    {
        private readonly IManagePersonBW _managePersonBW;

        public PersonController(IManagePersonBW managePersonBW)
        {
            _managePersonBW = managePersonBW;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var persons = await _managePersonBW.GetAllAsync();
            return Ok(persons);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var person = await _managePersonBW.GetByIdAsync(id);
            return person is not null ? Ok(person) : NotFound();
        }

        [HttpGet("dni/{dni}")]
        public async Task<IActionResult> GetByDni(string dni)
        {
            var person = await _managePersonBW.GetByDniAsync(dni);
            return person is not null ? Ok(person) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonDTO createPersonDTO)
        {
            var person = PersonMapper.CreatePersonDTOToPerson(createPersonDTO);
            var id = await _managePersonBW.CreateAsync(person);
            return CreatedAtAction(nameof(GetById), new { id }, person);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreatePersonDTO createPersonDTO)
        {
            var person = PersonMapper.CreatePersonDTOToPerson(createPersonDTO);
            var updated = await _managePersonBW.UpdateAsync(id, person);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _managePersonBW.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
