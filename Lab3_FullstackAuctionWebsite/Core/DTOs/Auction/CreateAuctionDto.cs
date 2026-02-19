using System.ComponentModel.DataAnnotations;

namespace Lab3_FullstackAuctionWebsite.Core.DTOs.Auction
{
    public class CreateAuctionDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal StartPrice { get; set; }
    }
}
