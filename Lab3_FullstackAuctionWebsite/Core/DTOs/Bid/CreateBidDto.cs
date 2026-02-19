using System.ComponentModel.DataAnnotations;

namespace Lab3_FullstackAuctionWebsite.Core.DTOs.Bid
{
    public class CreateBidDto
    {
        [Required]
        public int AuctionId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
