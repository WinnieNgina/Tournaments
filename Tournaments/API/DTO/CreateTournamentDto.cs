using ModelsLibrary.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.DTO;
public class CreateTournamentDto
{
    [Required(ErrorMessage = "Tournament name is required.")]
    [MaxLength(256, ErrorMessage = "Tournament name cannot exceed 256 characters.")]
    [DisplayName("Tournament Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Entry fee is required.")]
    [DisplayName("Entry Fee")]
    public decimal EntryFee { get; set; }

    [Required(ErrorMessage = "Teams limit is required.")]
    [DisplayName("Teams Limit")]
    public int TeamsLimit { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    [MaxLength(256, ErrorMessage = "Location cannot exceed 256 characters.")]
    [DisplayName("Location")]
    public string Location { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    [DisplayName("Description")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Tournament date is required.")]
    [DisplayName("Tournament Date")]
    public DateTime TournamentDate { get; set; }

    [Required(ErrorMessage = "Organizer ID is required.")]
    [DisplayName("Organizer ID")]
    public string OrganizerId { get; set; }

    [Required(ErrorMessage = "Tournament structure type is required.")]
    [DisplayName("Tournament Structure Type")]
    public TournamentStructureType StructureType { get; set; }
}
