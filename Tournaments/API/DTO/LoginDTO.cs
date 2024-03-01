using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class LoginDTO
    {
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null;

        [Display(Name = "User Name")]
        public string? UserName { get; set; } = null;

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
                return PhoneNumber ?? UserName;
            }
        }
    }
}