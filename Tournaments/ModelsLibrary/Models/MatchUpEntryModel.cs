namespace ModelsLibrary.Models;

public class MatchUpEntryModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string TeamCompetingId { get; set; }
    public virtual TeamModel TeamCompeting { get; set; }
    public double Score { get; set; }
    public string MatchUpId { get; set; }
    /// <summary>
    /// Represents the last matchup for the team
    /// </summary>
    public virtual MatchUpModel ParentMatchup { get; set; }

}
