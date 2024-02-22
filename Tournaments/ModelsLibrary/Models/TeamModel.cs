using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary.Models;

public class TeamModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the team.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the team.
    /// </summary>
    [MaxLength(200)]
    public required string Name { get; set; }

    /// <summary>
    /// This provides a brief introduction of the team, 
    /// what they do and for how long and their achievements so far
    /// </summary>
    [MaxLength(1000)]
    public required string Description { get; set; }

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
    public ICollection<PlayerTeamModel> Players { get; set; }
    /// <summary>
    /// Gets or sets the collection of participations representing the team's involvement in tournaments.
    /// </summary>
    public ICollection<ParticipationModel> Participations { get; set; }
}
