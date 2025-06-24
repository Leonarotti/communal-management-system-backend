using CommunalManagementSystem.BusinessWorkflow.Interfaces.DA;
using CommunalManagementSystem.DataAccess.Context;
using CommunalManagementSystem.DataAccess.DAOs;
using CommunalManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.DataAccess.Actions
{
    public class ManagePersonDA: IManagePersonDA
    {
        private readonly CommunalManagementSystemDbContext _context;

        public ManagePersonDA(CommunalManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Person person)
        {
            var personDAO = new PersonDAO
            {
                id = person.Id == Guid.Empty ? Guid.NewGuid() : person.Id,
                name = person.Name,
                dni = person.Dni,
                phone = person.Phone
            };

            _context.Persons.Add(personDAO);
            await _context.SaveChangesAsync();
            return personDAO.id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
                return false;

            _context.Persons.Remove(person);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            var persons = await _context.Persons.ToListAsync();
            return persons.Select(p => new Person
            {
                Id = p.id,
                Dni = p.dni,
                Name = p.name,
                Phone = p.phone,
                CreatedAt = p.created_at
            });
        }

        public async Task<Person?> GetByDniAsync(string dni)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.dni == dni);
            if (person == null)
                return null;

            return new Person
            {
                Id = person.id,
                Dni = person.dni,
                Name = person.name,
                Phone = person.phone,
                CreatedAt = person.created_at
            };
        }

        public async Task<Person?> GetByIdAsync(Guid id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
                return null;

            return new Person
            {
                Id = person.id,
                Dni = person.dni,
                Name = person.name,
                Phone = person.phone,
                CreatedAt = person.created_at
            };
        }

        public async Task<bool> UpdateAsync(Guid id, Person updatedPerson)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
                return false;

            person.name = updatedPerson.Name;
            person.dni = updatedPerson.Dni;
            person.phone = updatedPerson.Phone;

            _context.Persons.Update(person);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetTotalPersonsAsync()
        {
            return await _context.Persons.CountAsync();
        }
    }
}
