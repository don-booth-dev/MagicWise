using System.Collections.Generic;
using System.Text.Json;

namespace MagicWise.Integrations.ThemeParks.Models
{
    public class EntityDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        /// <summary>
        /// Attributes or variable payload returned for an entity.
        /// </summary>
        public JsonElement? Attributes { get; set; }

        /// <summary>
        /// Some endpoints return children inline; keep as optional.
        /// </summary>
        public List<EntityDto>? Children { get; set; }
    }
}
