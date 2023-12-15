using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DAL.Entities
{
    public class ApplicationUser:IdentityUser
    {
        
        public string State { get; set; }
        public string City { get; set; }
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public bool IsAgree { get; set; }
    }
}
