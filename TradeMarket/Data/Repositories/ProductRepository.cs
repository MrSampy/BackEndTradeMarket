using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductRepository:IProductRepository
    { 
        private readonly TradeMarketDbContext _context;
        public ProductRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            var result = _context.Products.ToList();
            return Task.FromResult<IEnumerable<Product>>(result);
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var result = _context.Products.First(x=>x.Id.Equals(id));
            return Task.FromResult(result);
        }

        public Task AddAsync(Product entity)
        {
            _context.Products.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(Product entity)
        {
            _context.Products.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            _context.Products.Remove(_context.Products.First(x=>x.Id.Equals(id)));
            return Task.CompletedTask;
            
        }

        public void Update(Product entity)
        {
            _context.Products.Update(entity);
        }

        public Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(_context.Products
                .Include(x=>x.Category.Products)
                .Include(x=>x.ReceiptDetails).ToList());
        }

        public Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return Task.FromResult(_context.Products
                .Include(x=>x.Category.Products)
                .Include(x=>x.ReceiptDetails).First(x=>x.Id.Equals(id)));
        }
    }
}