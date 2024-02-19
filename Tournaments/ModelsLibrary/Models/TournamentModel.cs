using ModelsLibrary.Models;

namespace ModelsLibrary
{
    public class TournamentModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tournament.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tournament.
        /// </summary>
        public string Name { get; set; }
        public decimal EntryFee { get; set; }

        /// <summary>
        /// Gets or sets the date of the tournament.
        /// </summary>
        public DateTime TournamentDate { get; set; }

        /// <summary>
        /// Gets or sets the ID of the tournament organizer.
        /// </summary>
        public string OrganizerId { get; set; }

        /// <summary>
        /// Gets or sets the TournamentOrganizerModel representing the organizer of the tournament.
        /// </summary>
        public TournamentOrganizerModel Organizer { get; set; }

        /// <summary>
        /// Gets or sets the collection of participations representing teams participating in the tournament.
        /// </summary>
        public virtual ICollection<ParticipationModel> Participations { get; set; }
        public double AverageRating { get; set; }
        /// <summary>
        /// Gets or sets the collection of reviews associated with the tournament.
        /// </summary>
        public virtual ICollection<ReviewModel> Reviews { get; set; }
        public virtual ICollection<TournamentPrizeModel> TournamentPrizes { get; set; }
    }
}