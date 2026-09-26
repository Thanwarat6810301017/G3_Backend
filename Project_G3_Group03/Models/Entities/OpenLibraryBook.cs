using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_G3_Group03.Models.Entities;

/// <summary>
/// Entity for books retrieved from Open Library API (Part B - API 1)
/// </summary>
[Table("OpenLibraryBooks")]
public class OpenLibraryBook
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string WorkKey { get; set; } = string.Empty; // e.g. /works/OL45804W

    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Author { get; set; }

    public int? FirstPublishYear { get; set; }

    [MaxLength(50)]
    public string? Isbn { get; set; }

    [MaxLength(1000)]
    public string? CoverUrl { get; set; }

    public string? RawJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
