using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicWise.Data.Entities;

public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public TagCategory Category { get; set; } = null!;

    public ICollection<DestinationTag> DestinationTags { get; set; } = new List<DestinationTag>();
}
