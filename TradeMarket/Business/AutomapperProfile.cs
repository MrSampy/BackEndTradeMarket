using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds,
                    r => 
                        r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();
            
            CreateMap<Product, ProductModel>()
                .ForMember(productModel=>productModel.CategoryName,
                    product=>
                        product.MapFrom(x=>x.Category.CategoryName))
                .ForMember(productModel => productModel.ReceiptDetailIds,
                    product =>
                        product.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            
            CreateMap<ReceiptDetail,ReceiptDetailModel>().ReverseMap();

            CreateMap<Customer, CustomerModel>()
                .ForMember(cm => cm.ReceiptsIds,
                    c => c.MapFrom(
                        x => x.Receipts.Select(t => t.Id)))
                .IncludeMembers(x=>x.Person)
                .ReverseMap();

            CreateMap<Person, CustomerModel>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(productCategoryModel => productCategoryModel.ProductIds,
                    productCategory =>
                        productCategory.MapFrom(x => x.Products.Select(product => product.Id)))
                .ReverseMap();
        }
    }
}