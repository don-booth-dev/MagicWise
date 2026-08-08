using System.Text.Json;

namespace MagicWise.Integrations.ThemeParks.Models
{
    public class DestinationDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Other fields returned by the API which are not modeled explicitly.
        /// </summary>
        public JsonElement? Raw { get; set; }
    }
}
