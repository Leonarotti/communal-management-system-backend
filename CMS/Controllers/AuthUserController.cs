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
    public class AuthUserController : ControllerBase
    {
        private readonly IManageAuthUserBW _manageAuthUserBW;

        public AuthUserController(IManageAuthUserBW manageAuthUserBW)
        {
            _manageAuthUserBW = manageAuthUserBW;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _manageAuthUserBW.GetAllAsync();
            var userDTOs = AuthUserMapper.AuthUsersToAuthUserDTOs(users);
            return Ok(userDTOs);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _manageAuthUserBW.GetByIdAsync(id);
            return user is not null
                ? Ok(AuthUserMapper.AuthUserToAuthUserDTO(user))
                : NotFound();
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _manageAuthUserBW.GetByEmailAsync(email);
            return user is not null
                ? Ok(AuthUserMapper.AuthUserToAuthUserDTO(user))
                : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthUserDTO createAuthUserDTO)
        {
            var authUser = AuthUserMapper.CreateAuthUserDTOToAuthUser(createAuthUserDTO);
            var id = await _manageAuthUserBW.CreateAsync(authUser);
            var createdUser = await _manageAuthUserBW.GetByIdAsync(id);
            if (createdUser is null) return NotFound();

            return CreatedAtAction(nameof(GetById), new { id }, AuthUserMapper.AuthUserToAuthUserDTO(createdUser));
        }

        [HttpPut("{id:guid}/password")]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] string newPassword)
        {
            var updated = await _manageAuthUserBW.UpdatePasswordAsync(id, newPassword);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _manageAuthUserBW.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateCredentials([FromBody] AuthCredentialsDTO credentials)
        {
            var isValid = await _manageAuthUserBW.ValidateCredentialsAsync(credentials.Email, credentials.Password);
            return Ok(isValid);
        }

        [HttpGet("with-persons")]
        public async Task<IActionResult> GetAllWithPersons()
        {
            var users = await _manageAuthUserBW.GetAllWithPersonsAsync();
            var dtoList = AuthUserMapper.AuthUsersToAuthUserWithPersonDTOs(users);
            return Ok(dtoList);
        }

        [HttpGet("with-person/email/{email}")]
        public async Task<IActionResult> GetWithPersonByEmail(string email)
        {
            var user = await _manageAuthUserBW.GetWithPersonByEmailAsync(email);
            return user is not null
                ? Ok(AuthUserMapper.AuthUserToAuthUserWithPersonDTO(user))
                : NotFound();
        }
    }
}