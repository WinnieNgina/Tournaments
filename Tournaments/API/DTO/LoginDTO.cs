using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class LoginDTO
    {
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        // Custom validation to ensure that either Email or UserName is provided
        [Required(ErrorMessage = "Either Email or User Name is required.")]
        public string LoginIdentifier
        {
            get
            {
                return Email ?? UserName;
            }
        }
    }
}
