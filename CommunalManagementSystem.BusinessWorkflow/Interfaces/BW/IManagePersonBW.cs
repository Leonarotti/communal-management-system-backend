using CommunalManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.BusinessWorkflow.Interfaces.BW
{
    public interface IManagePersonBW
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(Guid id);
        Task<Person?> GetByDniAsync(string dni);
        Task<Guid> CreateAsync(Person person);
        Task<bool> UpdateAsync(Guid id, Person updatedPerson);
        Task<bool> DeleteAsync(Guid id);
        Task<int> GetTotalPersonsAsync();
    }
}
