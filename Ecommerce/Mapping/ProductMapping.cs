using AutoMapper;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities.Product;

namespace Ecommerce.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDTO>().ForMember(
                x => x.CategoryName,
                op => op.MapFrom(src => src.Category.Name)).ReverseMap();
            CreateMap<Image, ImageDTO>().ReverseMap();
            CreateMap<AddProductDTO, Product>()
                .ForMember(m => m.Images, op => op.Ignore())
                .ReverseMap(); 
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(m => m.Images, op => op.Ignore())
                .ReverseMap();
        }
    }
}
