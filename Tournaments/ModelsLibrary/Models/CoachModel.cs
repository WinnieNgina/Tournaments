using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary.Models
{
    /// <summary>
    /// Represents a coach for teams in the application.
    /// </summary>
    public class CoachModel : User
    {
        /// <summary>
        /// Gets or sets the number of years of coaching experience.
        /// </summary>
        public int YearsOfExperience { get; set; }

        /// <summary>
        /// Gets or sets the URL to the coach's social media profile
        /// Can be twitter or linkedin or facebook.
        /// </summary>
        [MaxLength(1000)]
        public required string SocialMediaUrl { get; set; }

        /// <summary>
        /// Coaching specialization of the coach eg football.
        /// </summary>
        [MaxLength(500)]
        public required string CoachingSpecialization { get; set; }

        /// <summary>
        /// Highlights the achievements of the coach with a maximum 
        /// length of 500 characters in their specialization.
        /// </summary>
        [MaxLength(1000)]
        public string Achievements { get; set; }

        /// <summary>
        /// List of teams trained by the coach
        /// </summary>
        public ICollection<TeamModel> Teams { get; set; }
    }
}
