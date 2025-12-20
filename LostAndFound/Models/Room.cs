namespace LostAndFound.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Floor { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; } = null!;
        public List<FoundItem> FoundItems { get; set; } = [];
    }
}
