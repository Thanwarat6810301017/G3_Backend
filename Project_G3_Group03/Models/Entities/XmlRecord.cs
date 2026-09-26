using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_G3_Group03.Models.Entities;

/// <summary>
/// Entity for 1,000 Records XML Import (Bonus 3)
/// </summary>
[Table("XmlRecords")]
public class XmlRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string RecordId { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Category { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    public string? Description { get; set; }

    [MaxLength(50)]
    public string? RecordDate { get; set; }

    public string? RawXml { get; set; }

    public DateTime ImportedAt { get; set; } = DateTime.UtcNow;
}
