using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary.Models;

public class ReviewModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the review.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Sets the time when a review was created
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets additional context or details about the review.
    /// </summary>
    [MaxLength(1000)]
    public string Comment { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the associated tournament.
    /// </summary>
    public string TournamentId { get; set; }

    /// <summary>
    /// Gets or sets the TournamentModel representing the associated tournament.
    /// </summary>
    public virtual TournamentModel Tournament { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the person making the review.
    /// </summary>
    public string ReviewerId { get; set; }

    /// <summary>
    /// Gets or sets the UserModel representing the person making the review.
    /// </summary>
    public virtual User Reviewer { get; set; }

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
