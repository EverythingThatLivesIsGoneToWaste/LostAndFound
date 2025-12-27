using LostAndFound.Models;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.DTOs
{
    public class CreateItemDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Info { get; set; } = string.Empty;
        [Required]
        public DateTime FoundAtUtc { get; set; }
        [Required]
        public int RoomId { get; set; }
    }
}
