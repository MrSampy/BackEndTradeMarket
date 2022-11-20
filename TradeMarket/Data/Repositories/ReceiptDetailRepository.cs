using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ReceiptDetailRepository:IReceiptDetailRepository
    { 
        private readonly TradeMarketDbContext _context;
        public ReceiptDetailRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            var result = _context.ReceiptsDetails.ToList();
            return Task.FromResult<IEnumerable<ReceiptDetail>>(result);
        }

        public Task<ReceiptDetail> GetByIdAsync(int id)
        {
            var result = _context.ReceiptsDetails.First(x=>x.Id.Equals(id));
            return Task.FromResult(result);
        }

        public Task AddAsync(ReceiptDetail entity)
        {
            _context.ReceiptsDetails.Add(entity); 
            return Task.CompletedTask;
        }

        public void Delete(ReceiptDetail entity)
        {
            _context.ReceiptsDetails.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            _context.ReceiptsDetails.Remove(_context.ReceiptsDetails.First(x=>x.Id.Equals(id)));
            return Task.CompletedTask;
        }

        public void Update(ReceiptDetail entity)
        {
            _context.ReceiptsDetails.Update(entity);
        }

        public Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return Task.FromResult<IEnumerable<ReceiptDetail>>(_context.ReceiptsDetails
                .Include(x=>x.Product.Category)
                .Include(x=>x.Receipt.Customer).ToList());
        }
    }
}