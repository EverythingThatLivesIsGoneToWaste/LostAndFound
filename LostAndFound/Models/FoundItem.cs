namespace LostAndFound.Models
{
    public class FoundItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
        public DateTime FoundAt { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; } = null!;
    }
}
