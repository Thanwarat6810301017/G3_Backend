using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_G3_Group03.Models.Entities;

/// <summary>
/// Entity for cat facts retrieved from Cat Facts API (Part B - API 4)
/// </summary>
[Table("CatFacts")]
public class CatFact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Fact { get; set; } = string.Empty;

    public int Length { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
