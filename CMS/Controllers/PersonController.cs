using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.API.Mappers;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
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
            var personDTOs = PersonMapper.PersonsToPersonDTOs(persons);
            return Ok(personDTOs);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var person = await _managePersonBW.GetByIdAsync(id);
            return person is not null
                ? Ok(PersonMapper.PersonToPersonDTO(person))
                : NotFound();
        }

        [HttpGet("dni/{dni}")]
        public async Task<IActionResult> GetByDni(string dni)
        {
            var person = await _managePersonBW.GetByDniAsync(dni);
            return person is not null
                ? Ok(PersonMapper.PersonToPersonDTO(person))
                : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonDTO createPersonDTO)
        {
            var person = PersonMapper.CreatePersonDTOToPerson(createPersonDTO);
            var id = await _managePersonBW.CreateAsync(person);

            // Retornar solo el DTO generado
            var createdPerson = await _managePersonBW.GetByIdAsync(id);
            if (createdPerson == null) return NotFound();

            var dto = PersonMapper.PersonToPersonDTO(createdPerson);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
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
            try
            {
                var deleted = await _managePersonBW.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error eliminando la persona.", detail = ex.Message });
            }
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotalPersons()
        {
            var total = await _managePersonBW.GetTotalPersonsAsync();
            return Ok(total);
        }
    }
}
