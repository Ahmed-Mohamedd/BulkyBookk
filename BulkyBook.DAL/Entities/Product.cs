using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DAL.Entities
{
    public  class Product
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
        [Range(1,10000,ErrorMessage ="Maximum Price is 10000") ]
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

        public string ImageUrl { get; set; }    
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; } // navigational property
        [DisplayName("Cover Type")]
        public int CoverTypeId { get; set; }
        public CoverType CoverType { get; set; } // navigational property
    }
}


