using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    /// <summary>
    /// Represents a team in the application.
    /// </summary>
    public class TeamModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the team.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This provides a brief introduction of the team, 
        /// what they do and for how long and their achievements so far
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// ID of the team coach
        /// </summary>
        public string CoachId { get; set; }

        /// <summary>
        /// CoachModel representing the coach who coaches the team.
        /// </summary>
        public CoachModel Coach { get; set; }

        /// <summary>
        /// Team members
        /// </summary>
        public virtual ICollection<PlayerModel> Players { get; set; }
    }
}
