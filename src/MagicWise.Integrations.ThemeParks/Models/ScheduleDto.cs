using System.Collections.Generic;
using System.Text.Json;

namespace MagicWise.Integrations.ThemeParks.Models
{
    public class ScheduleDto
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        /// <summary>
        /// Entries typically contain opening/closing times, date info, etc. Keep flexible.
        /// </summary>
        public JsonElement? Entries { get; set; }
        public JsonElement? Raw { get; set; }
    }

    public class LiveStatusDto
    {
        /// <summary>
        /// Example: true when open. API may provide other fields like wait times.
        /// </summary>
        public bool? IsOpen { get; set; }
        public JsonElement? Details { get; set; }
    }
}
