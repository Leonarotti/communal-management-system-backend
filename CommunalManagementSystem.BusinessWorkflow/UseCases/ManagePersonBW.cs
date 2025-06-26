using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.DA;
using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.BusinessWorkflow.UseCases
{
    public class ManagePersonBW : IManagePersonBW
    {
        private readonly IManagePersonDA _managePersonDA;
        private readonly IManageQuotaBW _manageQuotaBW;

        public ManagePersonBW(IManagePersonDA managePersonDA, IManageQuotaBW manageQuotaBW)
        {
            _managePersonDA = managePersonDA;
            _manageQuotaBW = manageQuotaBW;
        }

        public async Task<Guid> CreateAsync(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.Name))
                throw new ArgumentException("El nombre no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(person.Dni))
                throw new ArgumentException("El DNI no puede estar vacío.");

            var existingPerson = await _managePersonDA.GetByDniAsync(person.Dni);
            if (existingPerson != null)
                throw new InvalidOperationException("Ya existe una persona con el mismo DNI.");

            person.Id = Guid.NewGuid();
            person.CreatedAt = DateTime.UtcNow;

            return await _managePersonDA.CreateAsync(person);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var quotaList = await _manageQuotaBW.GetByPersonAsync(id);
            if (quotaList.Any())
                throw new InvalidOperationException("No se puede eliminar una persona que tiene cuotas asociadas.");
            var existingPerson = await _managePersonDA.GetByIdAsync(id);
            if (existingPerson == null)
                throw new InvalidOperationException("No se encontró la persona para eliminar.");

            return await _managePersonDA.DeleteAsync(id);
        }

        public Task<IEnumerable<Person>> GetAllAsync()
        {
            return _managePersonDA.GetAllAsync();
        }

        public Task<Person?> GetByDniAsync(string dni)
        {
            return _managePersonDA.GetByDniAsync(dni);
        }

        public Task<Person?> GetByIdAsync(Guid id)
        {
            return _managePersonDA.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Guid id, Person updatedPerson)
        {
            var existingPerson = await _managePersonDA.GetByIdAsync(id);
            if (existingPerson == null)
                throw new InvalidOperationException("No se encontró la persona para actualizar.");

            if (string.IsNullOrWhiteSpace(updatedPerson.Name))
                throw new ArgumentException("El nombre no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(updatedPerson.Dni))
                throw new ArgumentException("El DNI no puede estar vacío.");

            // Validar que el nuevo DNI no esté en uso por otra persona
            var dniOwner = await _managePersonDA.GetByDniAsync(updatedPerson.Dni);
            if (dniOwner != null && dniOwner.Id != id)
                throw new InvalidOperationException("Ya existe otra persona con el mismo DNI.");

            updatedPerson.CreatedAt = existingPerson.CreatedAt;

            return await _managePersonDA.UpdateAsync(id, updatedPerson);
        }

        public async Task<int> GetTotalPersonsAsync()
        {
            return await _managePersonDA.GetTotalPersonsAsync();
        }
    }
}
