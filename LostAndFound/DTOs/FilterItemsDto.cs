namespace LostAndFound.DTOs
{
    public class FilterItemsDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Search { get; set; }
        public int? RoomId { get; set; }
    }
}
