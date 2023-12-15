using AutoMapper;
using BulkyBook.DAL.Entities;
using BulkyBook.PL.Helpers;
using BulkyBook.PL.ViewModels;
using Microsoft.Extensions.Configuration;


namespace BulkyBook.PL.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductCardViewModel, string>
    {
        
        public string Resolve(Product source, ProductCardViewModel destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
                return DocumetSetting.ImagePath(source.ImageUrl, "Images");
            return null;
        }

        
    }
}
