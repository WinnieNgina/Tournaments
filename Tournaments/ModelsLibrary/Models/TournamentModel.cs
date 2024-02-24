using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary.Models;

public class TournamentModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the tournament.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the tournament.
    /// </summary>
    [MaxLength(256)]
    public required string Name { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal EntryFee { get; set; }
    public required int TeamsLimit { get; set; }
    [MaxLength(256)]
    public required string Location { get; set; }
    /// <summary>
    /// Provides a brief description of the tournament
    /// </summary>
    [MaxLength(2000)]
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the date of the tournament.
    /// </summary>
    public DateTime TournamentDate { get; set; }

    /// <summary>
    /// Gets or sets the ID of the tournament organizer.
    /// </summary>
    public string OrganizerId { get; set; }

    /// <summary>
    /// Gets or sets the TournamentOrganizerModel representing the organizer of the tournament.
    /// </summary>
    public TournamentOrganizerModel Organizer { get; set; }

    /// <summary>
    /// Gets or sets the collection of participations representing teams participating in the tournament.
    /// </summary>
    public ICollection<ParticipationModel> Participations { get; set; }
    public double AverageRating { get; set; }
    /// <summary>
    /// Gets or sets the collection of reviews associated with the tournament.
    /// </summary>
    public ICollection<ReviewModel> Reviews { get; set; }
    public ICollection<TournamentPrizeModel> TournamentPrizes { get; set; }
    // Collection of match-ups associated with the tournament
    public ICollection<MatchUpModel> MatchUps { get; set; }
    public required TournamentStructureType StructureType { get; set; }
}
public enum TournamentStructureType
{
    RoundRobin,
    SingleElimination,
    DoubleElimination
}