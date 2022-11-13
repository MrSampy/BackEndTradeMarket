using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    
    public class ProductCategoryRepository:IProductCategoryRepository
    {
        private readonly TradeMarketDbContext _context;
        public ProductCategoryRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            var result = _context.ProductCategories.ToList();
            return Task.FromResult<IEnumerable<ProductCategory>>(result);
        }

        public Task<ProductCategory> GetByIdAsync(int id)
        {
            var result = _context.ProductCategories.First(x=>x.Id.Equals(id));
            return Task.FromResult(result);
        }

        public Task AddAsync(ProductCategory entity)
        {
            _context.ProductCategories.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(ProductCategory entity)
        {
            _context.ProductCategories.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            _context.ProductCategories.Remove(_context.ProductCategories.First(x=>x.Id.Equals(id)));
            return Task.CompletedTask;
            
        }

        public void Update(ProductCategory entity)
        {
            _context.ProductCategories.Update(entity);
        }
    }
    
}