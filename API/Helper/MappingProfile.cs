using API.Dtos;
using API.Dtos.Product;
using AutoMapper;
using Core.Entities;

namespace API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(c => c.Category, o => o.MapFrom(s => s.Category.Name))
                .ForMember(c => c.ImageUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<ProductCreateDto, Product>();
                
            CreateMap<Photo, PhotoToReturnDto>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<PhotoUrlResolver>());
        }
    }
}