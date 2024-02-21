namespace ModelsLibrary.Models
{
    public class TournamentPrizeModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TournamentId { get; set; }
        public virtual TournamentModel Tournament { get; set; }
        /// <summary>
        /// Gets or sets the collection of prizes associated with the tournament.
        /// </summary>
        public string PrizeId { get; set; }
        public virtual PrizeModel Prize { get; set; }
        public virtual ICollection<TournamentPrizeModel> TournamentPrizes { get; set; }

    }
}
