using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicWise.Data.Entities;

/// <summary>
/// Links an API destination id (string from the themeparks.wiki API) to a Tag.
/// All parks within the destination inherit these tags for filtering purposes.
/// </summary>
public class DestinationTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// The destination id as returned by the themeparks.wiki API (e.g. "waltdisneyworld").
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string DestinationId { get; set; } = null!;

    public int TagId { get; set; }

    [ForeignKey(nameof(TagId))]
    public Tag Tag { get; set; } = null!;
}
