using Lab3_FullstackAuctionWebsite.Core.DTOs.Bid;

namespace Lab3_FullstackAuctionWebsite.Core.DTOs.Auction
{
    public class AuctionResponseDto
    {
        public int AuctionId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal StartPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public string CreatorUserName { get; set; } = string.Empty;

        public decimal HighestBid { get; set; }

        public bool IsOpen { get; set; }

        public List<BidResponseDto> Bids { get; set; } = new();
    }
}
