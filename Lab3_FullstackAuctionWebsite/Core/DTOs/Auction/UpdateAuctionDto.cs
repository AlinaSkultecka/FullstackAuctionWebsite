using System.ComponentModel.DataAnnotations;

namespace Lab3_FullstackAuctionWebsite.Core.DTOs.Auction
{
    public class UpdateAuctionDto
    {
        [Required]
        public int AuctionId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime EndDate { get; set; }

        public decimal StartPrice { get; set; }
    }
}
