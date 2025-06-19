using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.API.Mappers
{
    public class PersonMapper
    {
        public static PersonDTO PersonToPersonDTO(Person person)
        {
            return new PersonDTO
            {
                _id = person.Id,
                dni = person.Dni,
                name = person.Name,
                phone = person.Phone,
                created_at = person.CreatedAt
            };
        }

        public static Person PersonDTOToPerson(PersonDTO personDTO)
        {
            return new Person
            {
                Id = personDTO._id,
                Dni = personDTO.dni,
                Name = personDTO.name,
                Phone = personDTO.phone,
                CreatedAt = personDTO.created_at
            };
        }

        public static IEnumerable<PersonDTO> PersonsToPersonDTOs(IEnumerable<Person> persons)
        {
            return persons.Select(PersonToPersonDTO);
        }

        public static Person CreatePersonDTOToPerson(CreatePersonDTO createPersonDTO)
        {
            return new Person
            {
                Dni = createPersonDTO.dni,
                Name = createPersonDTO.name,
                Phone = createPersonDTO.phone,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
