using API.Dtos;
using API.Dtos.Buying;
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
                .ForMember(c => c.Category, o => o.MapFrom(s => s.Category != null ? s.Category.Name : null))
                .ForMember(c => c.ImageUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<ProductCreateDto, Product>();
                
            CreateMap<Photo, PhotoToReturnDto>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<PhotoUrlResolver>());
            
            CreateMap<Item, ItemToReturnDto>()
                .ForMember(c => c.Brand, o => o.MapFrom(s => s.Brand != null ? s.Brand.Name : null))
                .ForMember(c => c.Model, o => o.MapFrom(s => s.Model != null ? s.Model.Name : null))
                .ForMember(c => c.Location, o => o.MapFrom(s => s.Location != null ? s.Location.Name : null))
                .ForMember(c => c.MobileNetwork, o => o.MapFrom(s => s.MobileNetwork != null ? s.MobileNetwork.Name : null))
                .ForMember(c => c.Storage, o => o.MapFrom(s => s.Storage != null ? s.Storage.Name : null))
                .ForMember(c => c.ItemType, o => o.MapFrom(s => s.ItemType != null ? s.ItemType.Name : null));

            CreateMap<Image, ImageDto>();    

            CreateMap<Buying, BuyingToReturnDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Items != null ? src.Items.Title : null))
                .ForMember(dest => dest.LocationName ,opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : null));
        }
    }
}