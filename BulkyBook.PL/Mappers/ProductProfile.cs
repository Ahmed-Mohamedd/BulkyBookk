using AutoMapper;
using BulkyBook.DAL.Entities;
using BulkyBook.PL.Helpers;
using BulkyBook.PL.ViewModels;


namespace BulkyBook.PL.Mappers
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductViewModel, Product>().ReverseMap();
            //.ForMember(p => p.ImageUrl, o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Product, ProductCardViewModel>()
                .ForMember(d => d.Category, (o => o.MapFrom(s => s.Category.Name)))
                .ForMember(d => d.CoverType, (o => o.MapFrom(s => s.CoverType.Name)))
                .ForMember(d => d.ImageName, (o => o.MapFrom(s => s.ImageUrl)))
                .ForMember(d => d.ImagePath, o => o.MapFrom<ProductPictureUrlResolver>());
                
                
        }
    }
}
