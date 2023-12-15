using System.ComponentModel.DataAnnotations;

namespace BulkyBook.PL.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [MinLength(5,ErrorMessage ="Minimum length for password is 5")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword Is Required")]
        [Compare("Password", ErrorMessage = "Confirm Password Doesn't Match Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string State { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }

        public string PostalCode { get; set; }

        public string? Role { get; set; }

        [Required(ErrorMessage = "IsAgree Is Required")]
        public bool IsAgree { get; set; }
    }
}
