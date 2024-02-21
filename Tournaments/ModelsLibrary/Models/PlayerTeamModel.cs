namespace ModelsLibrary.Models
{
    public class PlayerTeamModel
    {
        public string PlayerId { get; set; }
        public virtual PlayerModel Player { get; set; }

        public string TeamId { get; set; }
        public virtual TeamModel Team { get; set; }
    }
}
