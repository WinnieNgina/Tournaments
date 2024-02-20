namespace ModelsLibrary.Models;
public class MatchUpModel
{
    public string Id { get; set; }
    public string TournamentId { get; set; }
    public TournamentModel Tournament { get; set; }
    /// <summary>
    /// The set of teams that were involved in the match
    /// </summary>
    public virtual ICollection<MatchUpEntryModel> Entries { get; set; }

    public string WinningTeamId { get; set; }
    public TeamModel WinningTeam { get; set; }
    public int MatchUpRound { get; set; }
    public MatchStatus Status { get; set; }
}
public enum MatchStatus
{
    Scheduled,
    InProgress,
    Completed,
    Canceled
}
