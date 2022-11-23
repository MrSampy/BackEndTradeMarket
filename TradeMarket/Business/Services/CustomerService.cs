using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class CustomerService:ICustomerService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        
        public CustomerService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var result = await UnitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return Mapper.Map<IEnumerable<CustomerModel>>(result);
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var result = await UnitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            return  Mapper.Map<CustomerModel>(result);
            
        }
        public Task<IEnumerable<CustomerModel>> GetByProductIdAsync(int productId)
        {
            var result =  UnitOfWork.CustomerRepository.GetAllWithDetailsAsync()
                .Result.Where(x=>x.Receipts.Any(z=>z.ReceiptDetails.Any(c=>c.ProductId.Equals(productId))));
            return Task.FromResult(Mapper.Map<IEnumerable<CustomerModel>>(result));
            
        }
        public Task AddAsync(CustomerModel model)
        {
            if (model is null || model.Surname is null || model.Name == string.Empty || model.Surname == string.Empty
                              || model.DiscountValue<0
                              || DateTime.Now.Year - model.BirthDate.Year < 0
                              || DateTime.Now.Year - model.BirthDate.Year > 150)
                throw new MarketException();
            var customer = Mapper.Map<Customer>(model);
            UnitOfWork.CustomerRepository.AddAsync(customer);
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }
        public async Task UpdateAsync(CustomerModel model)
        {
            if (model is null)
                throw new MarketException("Customer is null.");

            if (model.Name == string.Empty || model.Surname == string.Empty)
                throw new MarketException("Name is empty.");

            if (DateTime.Now<model.BirthDate || DateTime.Now.Year - model.BirthDate.Year < 0  || DateTime.Now.Year - model.BirthDate.Year > 150)
                throw new MarketException("Date is not valid.");

            if (model.DiscountValue < 0)
                throw new MarketException("Discount is not valid.");

            var customer = Mapper.Map<Customer>(model);

            UnitOfWork.CustomerRepository.Update(customer);

            await UnitOfWork.SaveAsync();
        }

        public Task DeleteAsync(int modelId)
        {
            UnitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var result =  UnitOfWork.CustomerRepository.GetAllWithDetailsAsync().Result
                .Where(x=>x.Receipts.Any(c=>c.ReceiptDetails.Any(a=>a.ProductId.Equals(productId))));
            return Task.FromResult(Mapper.Map<IEnumerable<CustomerModel>>(result));
        }
    }
}