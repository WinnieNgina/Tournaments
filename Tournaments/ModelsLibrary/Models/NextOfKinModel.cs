﻿using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary.Models
{
    /// <summary>
    /// Represents information about the next of kin associated with one or more players.
    /// </summary>
    public class NextOfKinModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the next of kin.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the first name of the next of kin.
        /// </summary>
        [MaxLength(256)]
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the next of kin.
        /// </summary>
        [MaxLength(256)]
        public required string LastName { get; set; }

        /// <summary>
        /// Gets or sets the relationship of the next of kin to the player.
        /// </summary>
        [MaxLength(256)]
        public required string Relationship { get; set; }

        /// <summary>
        /// Gets or sets the contact information of the next of kin.
        /// </summary>
        [MaxLength(256)]
        public required string ContactInfo { get; set; }

        /// <summary>
        /// Gets or sets the collection of players associated with this next of kin.
        /// </summary>
        public ICollection<PlayerModel> Players { get; set; }
    }
}
