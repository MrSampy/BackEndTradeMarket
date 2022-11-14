using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CustomerRepository:ICustomerRepository
    {
        private readonly TradeMarketDbContext _context;
        public CustomerRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        
        public Task<Customer> GetByIdAsync(int id)
        {
            var result = _context.Customers.First(x=>x.Id.Equals(id));
            return Task.FromResult(result);
        }

        public Task AddAsync(Customer entity)
        {
            _context.Customers.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(Customer entity)
        {
            _context.Customers.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            _context.Customers.Remove(_context.Customers.First(x=>x.Id.Equals(id)));
            return Task.CompletedTask;        
        }

        public void Update(Customer entity)
        {
            _context.Customers.Update(entity);
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await _context.Customers
                .Include(x=>x.Person)
                .Include(x=>x.Receipts)
                .ThenInclude(x=>x.ReceiptDetails).ToListAsync();
        }

        public Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return Task.FromResult(_context.Customers
                .Include(x=>x.Person)
                .Include(x=>x.Receipts)
                .ThenInclude(x=>x.ReceiptDetails).First(x=>x.Id.Equals(id)));
        }
    }
}