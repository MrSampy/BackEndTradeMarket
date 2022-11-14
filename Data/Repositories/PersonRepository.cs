using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Data.Data;

namespace Data.Repositories
{
    public class PersonRepository:IPersonRepository
    {
        private readonly TradeMarketDbContext _context;
        public PersonRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Person>> GetAllAsync()
        {
            var result = _context.Persons.ToList();
            return Task.FromResult<IEnumerable<Person>>(result);
        }

        public Task<Person> GetByIdAsync(int id)
        {
            var result = _context.Persons.First(x=>x.Id.Equals(id));
            return Task.FromResult(result);
        }

        public Task AddAsync(Person entity)
        {
            _context.Persons.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(Person entity)
        {
            _context.Persons.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            _context.Persons.Remove(_context.Persons.First(x=>x.Id.Equals(id)));
            return Task.CompletedTask;
        }

        public void Update(Person entity)
        {
            _context.Persons.Update(entity);

        }
    }
}