using API.Dtos;
using API.Dtos.Item;
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
            
            CreateMap<Item, ItemToReturnDto>()
                .ForMember(c => c.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(c => c.Model, o => o.MapFrom(s => s.Model.Name))
                .ForMember(c => c.Location, o => o.MapFrom(s => s.Location.Name))
                .ForMember(c => c.MobileNetwork, o => o.MapFrom(s => s.MobileNetwork.Name))
                .ForMember(c => c.Storage, o => o.MapFrom(s => s.Storage.Name))
                .ForMember(c => c.ItemType, o => o.MapFrom(s => s.ItemType.Name));

            CreateMap<Image, ImageDto>();    
        }
    }
}