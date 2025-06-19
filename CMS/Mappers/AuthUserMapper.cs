using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.API.Mappers
{
    public class AuthUserMapper
    {
        public static AuthUserDTO AuthUserToAuthUserDTO(AuthUser user)
        {
            return new AuthUserDTO
            {
                _id = user.Id,
                email = user.Email,
                role = user.Role,
                person_id = user.PersonId,
                created_at = user.CreatedAt
            };
        }

        public static IEnumerable<AuthUserDTO> AuthUsersToAuthUserDTOs(IEnumerable<AuthUser> users)
        {
            return users.Select(AuthUserToAuthUserDTO);
        }

        public static AuthUser CreateAuthUserDTOToAuthUser(CreateAuthUserDTO createAuthUserDTO)
        {

            return new AuthUser
            {
                PersonId = createAuthUserDTO.person_id,
                Email = createAuthUserDTO.email,
                Password = createAuthUserDTO.password,
                Role = createAuthUserDTO.role
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



