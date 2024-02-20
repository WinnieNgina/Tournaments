namespace ModelsLibrary.Models
{
    public class PlayerTeamModel
    {
        public string PlayerId { get; set; }
        public PlayerModel Player { get; set; }

        public string TeamId { get; set; }
        public TeamModel Team { get; set; }
    }
}
