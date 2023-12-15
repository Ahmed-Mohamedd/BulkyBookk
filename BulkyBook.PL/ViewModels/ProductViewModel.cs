using BulkyBook.DAL.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.PL.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Maximum Price is 10000")]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Maximum Price for one product is 10000")]
        public double Price { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Maximum Price for a bulk order of 5 products is 10000")]
        public double Price5 { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Maximum Price for a bulk order of 10 products  is 10000")]
        public double Price10 { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }
      [DisplayName("Cover Type")]
        public int CoverTypeId { get; set; }


        
        public IFormFile Image { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

    }
}
