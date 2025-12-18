using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFound.Models
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public List<FoundItem> FoundItems { get; set; } = [];

        [NotMapped]
        public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;
    }
}
