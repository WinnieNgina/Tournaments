using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class SignUpOrganizerDTO
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Area of Residence")]
        public string AreaOfResidence { get; set; }
        [Required]
        [Display(Name = "Office or Organization Name")]
        public string OrganizationName { get; set; }
    }
}
