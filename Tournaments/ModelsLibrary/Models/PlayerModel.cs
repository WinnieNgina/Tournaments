﻿namespace ModelsLibrary.Models;

public class PlayerModel : User
{
    /// <summary>
    /// Gets or sets the date of birth of the player.
    /// </summary>
    public required DateTime DateOfBirth { get; set; }

    /// <summary>
    /// The collection of teams where a player is an active participant
    /// </summary>
    public ICollection<PlayerTeamModel> Teams { get; set; }
    /// <summary>
    /// ID of the player's next of kin
    /// </summary>
    public string NextOfKinId { get; set; }

    /// <summary>
    /// Gets or sets the next of kin associated with the player.
    /// </summary>

    public NextOfKinModel NextOfKin { get; set; }
    public required PlayerStatus Status { get; set; }
}
public enum PlayerStatus
{
    SearchingForTeam,
    HasTeam,
    Inactive
}
