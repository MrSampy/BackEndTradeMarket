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
    public class ProductService:IProductService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var result = await UnitOfWork.ProductRepository.GetAllWithDetailsAsync();
            return Mapper.Map<IEnumerable<ProductModel>>(result);
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var result = await UnitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            return  Mapper.Map<ProductModel>(result);
        }

        public Task AddAsync(ProductModel model)
        {
            if (model.ProductName == string.Empty || model.Price<0)
                throw new MarketException();
            UnitOfWork.ProductRepository.AddAsync(Mapper.Map<Product>(model));
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ProductModel model)
        {
            if (model.ProductName == string.Empty || model.Price<0)
                throw new MarketException();
            UnitOfWork.ProductRepository.Update(Mapper.Map<Product>(model));
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int modelId)
        {
            UnitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var list = GetAllAsync().Result;
            if (filterSearch.CategoryId != null)
                list =  list.Where(x => x.ProductCategoryId.Equals(filterSearch.CategoryId));
            if (filterSearch.MaxPrice != null)
                list = list.Where(x => x.Price <= filterSearch.MaxPrice);
            if (filterSearch.MinPrice != null)
                list = list.Where(x => x.Price >= filterSearch.MinPrice);
            return Task.FromResult(list);

        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var result = await UnitOfWork.ProductCategoryRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<ProductCategoryModel>>(result);
        }

        public Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (categoryModel.CategoryName == string.Empty)
                throw new MarketException();
            UnitOfWork.ProductCategoryRepository.AddAsync(Mapper.Map<ProductCategory>(categoryModel));
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (categoryModel.CategoryName == string.Empty)
                throw new MarketException();
            UnitOfWork.ProductCategoryRepository.Update(Mapper.Map<ProductCategory>(categoryModel));
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }

        public Task RemoveCategoryAsync(int categoryId)
        {
            UnitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            UnitOfWork.SaveAsync();
            return Task.CompletedTask;
        }
    }
}