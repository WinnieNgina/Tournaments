using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class SignUpCoachDTO
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
        [Display(Name = "Years of Coaching Experience")]
        public int YearsOfExperience { get; set; }
        [Required]
        [Display(Name = "LinkedIn URL")]
        [DataType(DataType.Url)]
        public string SocialMediaUrl { get; set; }
        [Required]
        [Display(Name = "Coaching Specialization")]
        public string CoachingSpecialization { get; set; }
        [Display(Name = "Achievements")]
        [StringLength(1000, ErrorMessage = "Achievements should be a maximum of  500 characters")]
        public string Achievements { get; set; }
        // Add other necessary fields if needed
    }
}
