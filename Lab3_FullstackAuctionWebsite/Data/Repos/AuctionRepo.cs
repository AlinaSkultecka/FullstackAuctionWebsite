using Lab3_FullstackAuctionWebsite.Data;
using Lab3_FullstackAuctionWebsite.Data.Entities;
using Lab3_FullstackAuctionWebsite.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab3_FullstackAuctionWebsite.Data.Repos
{
    public class AuctionRepo : IAuctionRepo
    {
        private readonly AppDbContext _context;

        public AuctionRepo(AppDbContext context)
        {
            _context = context;
        }

        // -------------------- GET ALL --------------------

        public async Task<List<Auction>> GetAllAsync()
        {
            return await _context.Auctions
                .Include(a => a.User)
                .Include(a => a.Bids)
                    .ThenInclude(b => b.User)
                .ToListAsync();
        }

        // -------------------- GET BY ID --------------------

        public async Task<Auction?> GetByIdAsync(int auctionId)
        {
            return await _context.Auctions
                .Include(a => a.User)
                .Include(a => a.Bids)
                    .ThenInclude(b => b.User)
                .SingleOrDefaultAsync(a => a.AuctionId == auctionId);
        }

        // -------------------- SEARCH BY TITLE --------------------

        public async Task<List<Auction>> SearchByTitleAsync(string title)
        {
            return await _context.Auctions
                .Include(a => a.User)
                .Include(a => a.Bids)
                .Where(a => a.Title.Contains(title))
                .ToListAsync();
        }

        // -------------------- ADD --------------------

        public async Task AddAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
            await _context.SaveChangesAsync();
        }

        // -------------------- UPDATE --------------------

        public async Task UpdateAsync(Auction auction)
        {
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
        }

        // -------------------- CHECK IF HAS BIDS --------------------

        public async Task<bool> HasBidsAsync(int auctionId)
        {
            return await _context.Bids
                .AnyAsync(b => b.AuctionId == auctionId);
        }


        public async Task<Auction> GetByIdWithUserAsync(int auctionId)
        {
            return await _context.Auctions
                .Include(a => a.User)
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);
        }

    }
}

