using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_G3_Group03.Models.Entities;

/// <summary>
/// Entity for books retrieved from Google Books API (Part B - API 2)
/// </summary>
[Table("GoogleBooks")]
public class GoogleBook
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string GoogleId { get; set; } = string.Empty; // Volume ID e.g. zyTCAlFPjgYC

    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Authors { get; set; }

    [MaxLength(255)]
    public string? Publisher { get; set; }

    [MaxLength(50)]
    public string? PublishedDate { get; set; }

    public string? Description { get; set; }

    public int? PageCount { get; set; }

    [MaxLength(255)]
    public string? Categories { get; set; }

    [MaxLength(1000)]
    public string? ThumbnailUrl { get; set; }

    public string? RawJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
