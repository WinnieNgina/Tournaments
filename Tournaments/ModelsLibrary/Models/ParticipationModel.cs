namespace ModelsLibrary.Models;
public class ParticipationModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the registration.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the registration is confirmed.
    /// </summary>
    public bool IsConfirmed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the team has attended the tournament.
    /// </summary>
    public bool HasAttended { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the associated team.
    /// </summary>
    public string TeamId { get; set; }

    /// <summary>
    /// Gets or sets the TeamModel representing the associated team.
    /// </summary>
    public TeamModel Team { get; set; }

    /// <summary>
    /// Id for the tournament
    /// </summary>
    public string TournamentId { get; set; }

    /// <summary>
    /// Tournament Model
    /// </summary>
    public TournamentModel Tournament { get; set; }
}
