namespace ModelsLibrary.Models;
public class MatchUpModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TournamentId { get; set; }
    public TournamentModel Tournament { get; set; }
    /// <summary>
    /// The set of teams that were involved in the match
    /// </summary>
    public ICollection<MatchUpEntryModel> Entries { get; set; }

    public string WinningTeamId { get; set; }
    public TeamModel WinningTeam { get; set; }
    public int MatchUpRound { get; set; }
    public required MatchStatus Status { get; set; }
}
public enum MatchStatus
{
    Scheduled,
    InProgress,
    Completed,
    Canceled
}
