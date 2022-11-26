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
    public class ReceiptService:IReceiptService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;


        public ReceiptService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var result = await UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            return Mapper.Map<IEnumerable<ReceiptModel>>(result);
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var result = await UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
            return  Mapper.Map<ReceiptModel>(result);
        }

        public Task AddAsync(ReceiptModel model)
        {
            UnitOfWork.ReceiptRepository.AddAsync( Mapper.Map<Receipt>(model));
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ReceiptModel model)
        {
            UnitOfWork.ReceiptRepository.Update(Mapper.Map<Receipt>(model));
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int modelId)
        {
            var receiptDetails = UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId).Result.ReceiptDetails;
            foreach (var receiptDetail in receiptDetails)
            {
                UnitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
            }
            
            UnitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
         {
             var receipt = await UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        
             if (receipt is null)
                 throw new MarketException("Receipt not found.");
 
             if (!(receipt.ReceiptDetails is null) && receipt.ReceiptDetails.Any(rd => rd.ProductId == productId))   
             {
                 var receiptDetail = receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);
 
                 receiptDetail.Quantity += quantity;
                 UnitOfWork.ReceiptDetailRepository.Update(receiptDetail);
             }
             else
             {
                 var product = await UnitOfWork.ProductRepository.GetByIdAsync(productId);
 
                 if (product is null)
                     throw new MarketException("Product not found.");
 
                 var entity = new ReceiptDetail()
                 {
                     ProductId = productId,
                     Product = product,
                     ReceiptId = receiptId,
                     Receipt = receipt,
                     Quantity = quantity,
                     UnitPrice = product.Price,
                     DiscountUnitPrice = product.Price * (1 - receipt.Customer.DiscountValue / 100m)
                 };
 
                 await UnitOfWork.ReceiptDetailRepository.AddAsync(entity);
             }
 
             await UnitOfWork.SaveAsync();
         }
        public Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receiptDetail = UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId).Result.ReceiptDetails
                .First(x=>x.ProductId.Equals(productId));
            receiptDetail.Quantity -= quantity;
            if (receiptDetail.Quantity <= 0)
            {
                UnitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
            }

            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var result = UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId).Result.ReceiptDetails;
            return Task.FromResult(Mapper.Map<IEnumerable<ReceiptDetailModel>>(result));
        }

        public Task<decimal> ToPayAsync(int receiptId)
        {
            var result = GetReceiptDetailsAsync(receiptId).Result
                .Select(x => x.DiscountUnitPrice*x.Quantity).Sum();
            return Task.FromResult(result);
        }     

        public Task CheckOutAsync(int receiptId)
        {
            var result = UnitOfWork.ReceiptRepository.GetByIdAsync(receiptId).Result;
            result.IsCheckedOut = true;
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var result = UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync().Result
                .Where(x => x.OperationDate >= startDate && x.OperationDate <= endDate);
            return Task.FromResult(Mapper.Map<IEnumerable<ReceiptModel>>(result));
        }
    }
}