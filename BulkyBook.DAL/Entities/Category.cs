using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DAL.Entities
{
    public  class Category
    {
        
        public int CategoryId { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string Name{ get; set; }
        [DisplayName("Dispaly Order")]
        [Range(1,100,ErrorMessage ="Display Order Should be in range of 1 to 100")]
        public int DisplayOrder{ get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;


    }
}
