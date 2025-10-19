using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.DA;
using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.BusinessWorkflow.UseCases
{
    public class ManageAuthUserBW : IManageAuthUserBW
    {
        private readonly IManageAuthUserDA _manageAuthUserDA;
        private readonly IManagePersonBW _managePersonBW;

        public ManageAuthUserBW(IManageAuthUserDA manageAuthUserDA, IManagePersonBW managePersonBW)
        {
            _manageAuthUserDA = manageAuthUserDA;
            _managePersonBW = managePersonBW;
        }

        public Task<IEnumerable<AuthUser>> GetAllAsync()
        {
            return _manageAuthUserDA.GetAllAsync();
        }

        public Task<AuthUser?> GetByIdAsync(Guid id)
        {
            return _manageAuthUserDA.GetByIdAsync(id);
        }

        public Task<AuthUser?> GetByEmailAsync(string email)
        {
            return _manageAuthUserDA.GetByEmailAsync(email);
        }

        public Task<Guid> CreateAsync(AuthUser authUser)
        {
            authUser.Id = Guid.NewGuid();
            authUser.CreatedAt = DateTime.UtcNow;
            return _manageAuthUserDA.CreateAsync(authUser);
        }

        public Task<bool> UpdatePasswordAsync(Guid id, string newPassword)
        {
            return _manageAuthUserDA.UpdatePasswordAsync(id, newPassword);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return _manageAuthUserDA.DeleteAsync(id);
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _manageAuthUserDA.GetByEmailAsync(email);
            if (user == null) return false;

            // 👇 Si implementas hashing, aquí sería el lugar para validar
            return user.Password == password;
        }

        public async Task<IEnumerable<AuthUserWithPerson>> GetAllWithPersonsAsync()
        {
            var authUsers = await _manageAuthUserDA.GetAllAsync(); // authUsers sin navegación
            var persons = await _managePersonBW.GetAllAsync();     // lista de persons

            // Convertimos lista de persons a diccionario para acceso rápido
            var personDict = persons.ToDictionary(p => p.Id);

            var result = new List<AuthUserWithPerson>();

            foreach (var user in authUsers)
            {
                if (!personDict.TryGetValue(user.Id, out var person))
                    continue; // O decide lanzar excepción o retornar nulo

                result.Add(new AuthUserWithPerson
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,

                    PersonId = person.Id,
                    PersonName = person.Name,
                    PersonDni = person.Dni,
                    PersonPhone = person.Phone,
                });
            }

            return result;
        }

        public async Task<AuthUserWithPerson?> GetWithPersonByEmailAsync(string email)
        {
            var user = await _manageAuthUserDA.GetByEmailAsync(email);
            if (user == null) return null;

            var person = await _managePersonBW.GetByIdAsync(user.Id);
            if (person == null) return null;

            return new AuthUserWithPerson
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,

                PersonId = person.Id,
                PersonName = person.Name,
                PersonDni = person.Dni,
                PersonPhone = person.Phone,
            };
        }
    }
}
