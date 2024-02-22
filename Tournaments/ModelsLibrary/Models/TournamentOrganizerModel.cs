using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary.Models;

public class TournamentOrganizerModel : User
{
    /// <summary>
    /// Gets or sets the organization name or office of the tournament organizer. 
    /// </summary>
    [MaxLength(200)]
    public required string OrganizationName { get; set; }
    /// <summary>
    /// Gets or sets the collection of tournaments organized by this organizer.
    /// </summary>
    public ICollection<TournamentModel> Tournaments { get; set; }

}
