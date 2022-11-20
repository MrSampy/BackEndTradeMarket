using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        CustomerRepository customerRepository;
        PersonRepository personRepository;
        ProductCategoryRepository productCategoryRepository;
        ProductRepository productRepository;
        ReceiptDetailRepository receiptDetailRepository;
        ReceiptRepository receiptRepository;
        readonly TradeMarketDbContext dbContext;

        public UnitOfWork(TradeMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ICustomerRepository CustomerRepository => customerRepository ??= new CustomerRepository(dbContext);

        public IPersonRepository PersonRepository => personRepository ??= new PersonRepository(dbContext);

        public IProductRepository ProductRepository => productRepository ??= new ProductRepository(dbContext);

        public IProductCategoryRepository ProductCategoryRepository => productCategoryRepository ??= new ProductCategoryRepository(dbContext);

        public IReceiptRepository ReceiptRepository => receiptRepository ??= new ReceiptRepository(dbContext);

        public IReceiptDetailRepository ReceiptDetailRepository => receiptDetailRepository ??= new ReceiptDetailRepository(dbContext);

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
