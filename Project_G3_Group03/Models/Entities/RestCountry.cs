using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_G3_Group03.Models.Entities;

/// <summary>
/// Entity for country data retrieved from REST Countries API (Part B - API 3)
/// </summary>
[Table("Countries")]
public class RestCountry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string CommonName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? OfficialName { get; set; }

    [MaxLength(10)]
    public string? Cca2 { get; set; }

    [MaxLength(10)]
    public string? Cca3 { get; set; }

    [MaxLength(255)]
    public string? Capital { get; set; }

    [MaxLength(100)]
    public string? Region { get; set; }

    [MaxLength(100)]
    public string? Subregion { get; set; }

    public long? Population { get; set; }

    public double? Area { get; set; }

    [MaxLength(1000)]
    public string? FlagUrl { get; set; }

    public string? RawJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
