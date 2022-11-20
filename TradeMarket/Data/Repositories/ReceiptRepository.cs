using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ReceiptRepository:IReceiptRepository
    {        
        private readonly TradeMarketDbContext _context;
        public ReceiptRepository(TradeMarketDbContext context)
        {
            _context = context;
            
        }

        public Task<IEnumerable<Receipt>> GetAllAsync()
        {
            var result = _context.Receipts.ToList();
            return Task.FromResult<IEnumerable<Receipt>>(result);
        }

        public Task<Receipt> GetByIdAsync(int id)
        {
            var result = _context.Receipts.First(x=>x.Id.Equals(id));
            return Task.FromResult(result);
        }

        public Task AddAsync(Receipt entity)
        {
            _context.Receipts.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(Receipt entity)
        {
            _context.Receipts.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            _context.Receipts.Remove(_context.Receipts.First(x=>x.Id.Equals(id)));
            return Task.CompletedTask;
            
        }

        public void Update(Receipt entity)
        {
            _context.Receipts.Update(entity);
        }

        public Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return Task.FromResult<IEnumerable<Receipt>>(_context.Receipts
                .Include(x => x.Customer.Person)
                .Include(x => x.ReceiptDetails)
                .ThenInclude(x=>x.Product.Category).ToList());
        }

        public Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return Task.FromResult(_context.Receipts
                .Include(x => x.Customer.Person)
                .Include(x => x.ReceiptDetails)
                .ThenInclude(x=>x.Product.Category).First(x=>x.Id.Equals(id)));
        }
    }
}