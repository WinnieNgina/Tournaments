namespace ModelsLibrary
{
    /// <summary>
    /// Represents a player in the application with a relationship to their next of kin.
    /// </summary>
    public class PlayerModel : UserModel
    {
        /// <summary>
        /// Gets or sets the date of birth of the player.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// The collection of teams where a player is an active participant
        /// </summary>
        public virtual ICollection<TeamModel> Teams { get; set; }
        /// <summary>
        /// ID of the player's next of kin
        /// </summary>
        public string NextOfKinId { get; set; }

        /// <summary>
        /// Gets or sets the next of kin associated with the player.
        /// </summary>

        public NextOfKinModel NextOfKin { get; set; }
        public PlayerStatus Status { get; set; }
    }
    public enum PlayerStatus
    {
        SearchingForTeam,
        HasTeam,
        Inactive
    }
}
