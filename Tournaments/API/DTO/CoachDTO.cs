using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class CoachDTO
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string AreaOfResidence { get; set; }
    public int YearsOfExperience { get; set; }
    public string SocialMediaUrl { get; set; }
    public string CoachingSpecialization { get; set; }
    public string Achievements { get; set; }


}
