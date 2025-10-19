using CommunalManagementSystem.BusinessWorkflow.Interfaces.DA;
using CommunalManagementSystem.DataAccess.Context;
using CommunalManagementSystem.DataAccess.DAOs;
using CommunalManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunalManagementSystem.DataAccess.Actions
{
    public class ManageAuthUserDA : IManageAuthUserDA
    {
        private readonly CommunalManagementSystemDbContext _context;

        public ManageAuthUserDA(CommunalManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(AuthUser authUser)
        {
            var authUserDAO = new AuthUserDAO
            {
                id = authUser.Id != Guid.Empty ? authUser.Id : Guid.NewGuid(),
                person_id = authUser.Id,
                email = authUser.Email,
                password = authUser.Password,
                role = authUser.Role,
                created_at = DateTime.UtcNow
            };

            _context.AuthUsers.Add(authUserDAO);
            await _context.SaveChangesAsync();
            return authUserDAO.id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.AuthUsers.FindAsync(id);
            if (user == null)
                return false;

            _context.AuthUsers.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<AuthUser?> GetByIdAsync(Guid id)
        {
            var user = await _context.AuthUsers.FindAsync(id);
            return user is null ? null : MapToDomain(user);
        }

        public async Task<AuthUser?> GetByEmailAsync(string email)
        {
            var user = await _context.AuthUsers
                .FirstOrDefaultAsync(u => u.email == email);
            return user is null ? null : MapToDomain(user);
        }

        public async Task<IEnumerable<AuthUser>> GetAllAsync()
        {
            var users = await _context.AuthUsers.ToListAsync();
            return users.Select(MapToDomain);
        }

        public async Task<bool> UpdateAsync(Guid id, AuthUser updatedUser)
        {
            var existing = await _context.AuthUsers.FindAsync(id);
            if (existing == null) return false;

            existing.email = updatedUser.Email;
            existing.password = updatedUser.Password;
            existing.role = updatedUser.Role;
            existing.person_id = updatedUser.Id;

            _context.AuthUsers.Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        private static AuthUser MapToDomain(AuthUserDAO dao)
        {
            return new AuthUser
            {
                Id = dao.id,
                PersonId = dao.person_id,
                Email = dao.email,
                Password = dao.password,
                Role = dao.role,
                CreatedAt = dao.created_at
            };
        }

        public async Task<bool> UpdatePasswordAsync(Guid id, string newPassword)
        {
            var user = await _context.AuthUsers.FindAsync(id);
            if (user == null)
                return false;

            user.password = newPassword;
            _context.AuthUsers.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
