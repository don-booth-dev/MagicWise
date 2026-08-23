using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicWise.Data.Entities;

public class TagCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public int DisplayOrder { get; set; }

    /// <summary>
    /// Controls whether this category's filter facet is shown in the left panel.
    /// </summary>
    public bool IsVisible { get; set; } = true;

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
