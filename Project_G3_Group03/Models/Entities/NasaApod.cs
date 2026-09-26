using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_G3_Group03.Models.Entities;

/// <summary>
/// Entity for NASA Astronomy Picture of the Day (Part B - API 5)
/// </summary>
[Table("NasaApods")]
public class NasaApod
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Date { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    public string? Explanation { get; set; }

    [MaxLength(1000)]
    public string? Url { get; set; }

    [MaxLength(1000)]
    public string? HdUrl { get; set; }

    [MaxLength(50)]
    public string? MediaType { get; set; }

    [MaxLength(255)]
    public string? Copyright { get; set; }

    public string? RawJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
