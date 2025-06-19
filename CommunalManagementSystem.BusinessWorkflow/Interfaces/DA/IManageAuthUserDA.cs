using CommunalManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.BusinessWorkflow.Interfaces.DA
{
    public interface IManageAuthUserDA
    {
        Task<Guid> CreateAsync(AuthUser authUser);
        Task<bool> UpdateAsync(Guid id, AuthUser updatedUser);
        Task<bool> DeleteAsync(Guid id);
        Task<AuthUser?> GetByIdAsync(Guid id);
        Task<AuthUser?> GetByEmailAsync(string email);
        Task<IEnumerable<AuthUser>> GetAllAsync();
        Task<bool> UpdatePasswordAsync(Guid id, string newPassword);
    }
}