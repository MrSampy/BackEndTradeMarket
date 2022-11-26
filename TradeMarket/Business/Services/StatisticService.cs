using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class StatisticService:IStatisticService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper Mapper;

        public StatisticService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {           
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var list = (await UnitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync())
                .GroupBy(t => t.Product, t => t.Quantity,
                    (product, quantity) => new
                    {
                        Product = product,
                        Count = quantity.Sum()
                    })
                .OrderByDescending(t => t.Count)
                .Select(t => t.Product)
                .Take(productCount);

            return Mapper.Map<IEnumerable<ProductModel>>(list);
        }
        public  Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync().Result
                .Where(x => x.CustomerId.Equals(customerId));
            var list = new List<ReceiptDetail>();
            foreach (var receipt in receipts)
                list.AddRange(receipt.ReceiptDetails);
            
            var result = list.GroupBy(t => t.Product, t => t.Quantity,
                    (product, quantity) => new
                    {
                        Product = product,
                        Count = quantity.Sum()
                    })
                .OrderByDescending(t => t.Count)
                .Select(t => t.Product)
                .Take(productCount);

            return Task.FromResult(Mapper.Map<IEnumerable<ProductModel>>(result));

        }

        public Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var list = UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync().Result
                .Where(x => x.OperationDate <= endDate && x.OperationDate >= startDate)
                .GroupBy(x=>x.Customer,x=>x.ReceiptDetails.Select(y=>y.Quantity*y.DiscountUnitPrice).Sum(),
                (customer,sum)=> new CustomerActivityModel
                {
                    CustomerId = customer.Id,
                    ReceiptSum = sum.Sum(),
                    CustomerName = customer.Person.Name +" "+ customer.Person.Surname
                })
                .OrderByDescending(x=>x.ReceiptSum).Take(customerCount).Select(x=>x);
            return Task.FromResult(list);
        }

         public Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var list = UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync().Result
                .Where(x => x.OperationDate <= endDate && x.OperationDate >= startDate
                                                       && x.ReceiptDetails.Any(a =>
                                                           a.Product.Category.Id.Equals(categoryId))).Select(x=>x.ReceiptDetails);
            var receiptDetails = new List<ReceiptDetail>();
            foreach (var entity in list)
            {
                receiptDetails.AddRange(entity);
            }

            var result = receiptDetails.Where(t => t.Product.Category.Id.Equals(categoryId)).Sum(t => t.DiscountUnitPrice * t.Quantity);
            return Task.FromResult(result);

        }
    }
}