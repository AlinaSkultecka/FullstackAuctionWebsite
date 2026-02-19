using Lab3_FullstackAuctionWebsite.Core.DTOs.Bid;

namespace Lab3_FullstackAuctionWebsite.Core.Interfaces
{
    public interface IBidService
    {
        Task<BidResponseDto?> CreateAsync(CreateBidDto dto, int userId);

        Task<bool> DeleteLatestBidAsync(int auctionId, int userId);

        Task<List<BidResponseDto>> GetByAuctionIdAsync(int auctionId);
    }
}
