using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.API.Mappers
{
    public class AuthUserMapper
    {
        public static AuthUser createAuthUserDTOToAuthUser(CreateAuthUserDTO createAuthUserDTO)
        {
            return new AuthUser
            {
                PersonId = createAuthUserDTO.PersonId,
                Email = createAuthUserDTO.Email,
                Password = createAuthUserDTO.Password,
                Role = createAuthUserDTO.Role
            };
        }

        public static AuthUserWithPersonDTO AuthUserToAuthUserWithPersonDTO(AuthUserWithPerson authUserWithPerson)
        {
            return new AuthUserWithPersonDTO
            {
                _id = authUserWithPerson.Id,
                email = authUserWithPerson.Email,
                role = authUserWithPerson.Role,
                created_at = authUserWithPerson.CreatedAt,
                person_id = authUserWithPerson.PersonId,
                person_name = authUserWithPerson.PersonName,
                person_dni = authUserWithPerson.PersonDni,
                person_phone = authUserWithPerson.PersonPhone
            };
        }

        public static IEnumerable<AuthUserWithPersonDTO> AuthUsersToAuthUserWithPersonDTOs(IEnumerable<AuthUserWithPerson> authUsersWithPerson)
        {
            return authUsersWithPerson.Select(AuthUserToAuthUserWithPersonDTO);
        }

 
    }
}



