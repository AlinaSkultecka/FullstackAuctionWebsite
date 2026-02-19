using Lab3_FullstackAuctionWebsite.Core.DTOs.Auction;

namespace Lab3_FullstackAuctionWebsite.Core.Interfaces
    {
        public interface IAuctionService
        {
            // Get all auctions
            Task<List<AuctionResponseDto>> GetAllAsync();

            // Get single auction with bids
            Task<AuctionResponseDto?> GetByIdAsync(int id);

            // Search open auctions by title
            Task<List<AuctionResponseDto>> SearchOpenAsync(string title);

            // Create auction (logged in user)
            Task<AuctionResponseDto> CreateAsync(CreateAuctionDto dto, int userId);

            // Update auction
            Task<bool> UpdateAsync(UpdateAuctionDto dto, int userId);

            // Deactivate auction (admin only)
            Task<bool> DeactivateAsync(int auctionId);
        }
}
