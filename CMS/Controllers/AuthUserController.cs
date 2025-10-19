using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.API.Mappers;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using CommunalManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using BCrypt.Net;

namespace CommunalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IManageAuthUserBW _manageAuthUserBW;
        private readonly IConfiguration _configuration;

        public AuthUserController(IManageAuthUserBW manageAuthUserBW, IConfiguration configuration)
        {
            _manageAuthUserBW = manageAuthUserBW;
            _configuration = configuration;
        }

        // ------------------------------------
        // 🔹 GET: api/authuser
        // ------------------------------------
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

        // ------------------------------------
        // 🔹 POST: api/authuser
        // ------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthUserDTO createAuthUserDTO)
        {
            var authUser = AuthUserMapper.CreateAuthUserDTOToAuthUser(createAuthUserDTO);

            // ✅ Encriptar contraseña antes de guardar
            authUser.Password = BCrypt.Net.BCrypt.HashPassword(createAuthUserDTO.password);

            var id = await _manageAuthUserBW.CreateAsync(authUser);
            var createdUser = await _manageAuthUserBW.GetByIdAsync(id);
            if (createdUser is null) return NotFound();

            return CreatedAtAction(nameof(GetById), new { id }, AuthUserMapper.AuthUserToAuthUserDTO(createdUser));
        }

        // ------------------------------------
        // 🔹 PUT: api/authuser/{id}/password  (versión segura)
        // ------------------------------------
        public class ChangePasswordRequest
        {
            public string CurrentPassword { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
        }

        [HttpPut("{id:guid}/password")]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] ChangePasswordRequest body)
        {
            if (string.IsNullOrWhiteSpace(body.CurrentPassword) || string.IsNullOrWhiteSpace(body.NewPassword))
                return BadRequest("Ambas contraseñas son requeridas.");

            var user = await _manageAuthUserBW.GetByIdAsync(id);
            if (user == null)
                return NotFound("Usuario no encontrado.");

            // 🔒 Verificar la contraseña actual
            bool isCurrentValid = BCrypt.Net.BCrypt.Verify(body.CurrentPassword, user.Password);
            if (!isCurrentValid)
                return BadRequest("La contraseña actual es incorrecta.");

            // 🔐 Encriptar y guardar la nueva contraseña
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(body.NewPassword);
            bool updated = await _manageAuthUserBW.UpdatePasswordAsync(id, hashedPassword);

            return updated ? NoContent() : StatusCode(500, "Error al actualizar la contraseña.");
        }

        // ------------------------------------
        // 🔹 DELETE: api/authuser/{id}
        // ------------------------------------
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _manageAuthUserBW.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        // ------------------------------------
        // 🔹 POST: api/authuser/validate
        // ------------------------------------
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateCredentials([FromBody] AuthCredentialsDTO credentials)
        {
            var isValid = await _manageAuthUserBW.ValidateCredentialsAsync(credentials.Email, credentials.Password);
            return Ok(isValid);
        }

        // ------------------------------------
        // 🔹 GET: api/authuser/with-persons
        // ------------------------------------
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

        // ------------------------------------
        // 🔹 POST: api/authuser/login
        // ------------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthCredentialsDTO credentials)
        {
            var user = await _manageAuthUserBW.GetByEmailAsync(credentials.Email);
            if (user == null)
                return Unauthorized(new { message = "Usuario no encontrado" });

            // ✅ Verificar con BCrypt
            bool valid = BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password);
            if (!valid)
                return Unauthorized(new { message = "Contraseña incorrecta" });

            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var credentialsJwt = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("userId", user.Id.ToString()),
                new Claim("role", user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentialsJwt
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    role = user.Role
                }
            });
        }
    }
}
