using Lab3_FullstackAuctionWebsite.Data.Entities;

namespace Lab3_FullstackAuctionWebsite.Data.Interfaces
{
    public interface IAuctionRepo
    {
        Task<List<Auction>> GetAllAsync();

        Task<Auction?> GetByIdAsync(int auctionId);

        Task<List<Auction>> SearchByTitleAsync(string title);

        Task AddAsync(Auction auction);

        Task UpdateAsync(Auction auction);

        Task<bool> HasBidsAsync(int auctionId);

        Task<Auction?> GetByIdWithUserAsync(int auctionId);
    }
}
