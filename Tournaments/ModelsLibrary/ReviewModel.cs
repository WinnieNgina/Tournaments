namespace ModelsLibrary
{
    public class ReviewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets additional context or details about the review.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated tournament.
        /// </summary>
        public string TournamentId { get; set; }

        /// <summary>
        /// Gets or sets the TournamentModel representing the associated tournament.
        /// </summary>
        public TournamentModel Tournament { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the person making the review.
        /// </summary>
        public string ReviewerId { get; set; }

        /// <summary>
        /// Gets or sets the UserModel representing the person making the review.
        /// </summary>
        public virtual UserModel Reviewer { get; set; }

        /// <summary>
        /// Gets or sets the rating given in the review.
        /// </summary>
        public Rating Rating { get; set; }
    }

    /// <summary>
    /// Enum representing different rating levels.
    /// </summary>
    public enum Rating
    {
        Bad,
        Poor,
        Average,
        Good,
        VeryGood
    }
}
