using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary.Models;

public class PrizeModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the prize.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the title or name of the prize.
    /// </summary>
    [MaxLength(100)]
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the prize.
    /// </summary>
    [MaxLength(500)]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the value of the prize.
    /// </summary>
    public decimal PrizeAmount { get; set; }

    /// <summary>
    /// Gets or sets the percentage of the tournament entry fees allocated to this prize.
    /// </summary>
    public double PrizePercentage { get; set; }

    /// <summary>
    /// Gets or sets the type of the prize (e.g., cash, merchandise, etc.).
    /// </summary>
    [MaxLength(200)]
    public required string PrizeType { get; set; }
}
