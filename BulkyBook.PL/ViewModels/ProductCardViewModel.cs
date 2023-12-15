using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.PL.ViewModels
{
    public class ProductCardViewModel
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
      

        public string ImagePath { get; set; }

        public string ImageName { get; set; }

        public string Category { get; set; }
        public string CoverType { get; set; }
        public int Count { get; set; }=1;

    }
}
