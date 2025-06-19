using CommunalManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.BusinessWorkflow.Interfaces.BW
{
    public interface IManageAuthUserBW
    {
        Task<IEnumerable<AuthUser>> GetAllAsync();
        Task<AuthUser?> GetByIdAsync(Guid id);
        Task<AuthUser?> GetByEmailAsync(string email);
        Task<Guid> CreateAsync(AuthUser authUser);
        Task<bool> UpdatePasswordAsync(Guid id, string newPassword);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ValidateCredentialsAsync(string email, string password);
        Task<IEnumerable<AuthUserWithPerson>> GetAllWithPersonsAsync();
        Task<AuthUserWithPerson?> GetWithPersonByEmailAsync(string email);
    }
}
