namespace ModelsLibrary.Models;

public class MatchUpEntryModel
{
    public string Id { get; set; }
    
    public string TeamCompetingId { get; set; }
    public TeamModel TeamCompeting { get; set; }
    public double Score { get; set; }
    public string MatchUpId { get; set; }
    /// <summary>
    /// Represents the last matchup for the team
    /// </summary>
    public MatchUpModel ParentMatchup { get; set; }

}
