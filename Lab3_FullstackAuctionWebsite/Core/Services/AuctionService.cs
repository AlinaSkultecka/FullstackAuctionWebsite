using AutoMapper;
using Lab3_FullstackAuctionWebsite.Core.DTOs;
using Lab3_FullstackAuctionWebsite.Core.DTOs.Auction;
using Lab3_FullstackAuctionWebsite.Core.Interfaces;
using Lab3_FullstackAuctionWebsite.Data.Entities;
using Lab3_FullstackAuctionWebsite.Data.Interfaces;

namespace Lab3_FullstackAuctionWebsite.Core.Services
{
    public class AuctionService : IAuctionService
    {
        private const int AuctionDurationDays = 14;

        private readonly IAuctionRepo _auctionRepo;
        private readonly IMapper _mapper;

        public AuctionService(IAuctionRepo auctionRepo, IMapper mapper)
        {
            _auctionRepo = auctionRepo;
            _mapper = mapper;
        }

        // -------------------- GET ALL --------------------

        public async Task<List<AuctionResponseDto>> GetAllAsync()
        {
            var auctions = await _auctionRepo.GetAllAsync();

            var result = auctions.Select(a =>
            {
                var dto = _mapper.Map<AuctionResponseDto>(a);

                dto.HighestBid = a.Bids.Any()
                    ? a.Bids.Max(b => b.Amount)
                    : a.StartPrice;

                dto.IsOpen = a.EndDate > DateTime.UtcNow && a.IsActive;

                dto.CreatorUserName = a.User.UserName;

                return dto;
            }).ToList();

            return result;
        }

        // -------------------- GET BY ID --------------------

        public async Task<AuctionResponseDto?> GetByIdAsync(int auctionId)
        {
            var auction = await _auctionRepo.GetByIdAsync(auctionId);

            if (auction == null)
                return null;

            var dto = _mapper.Map<AuctionResponseDto>(auction);

            dto.HighestBid = auction.Bids.Any()
                ? auction.Bids.Max(b => b.Amount)
                : auction.StartPrice;

            dto.IsOpen = auction.EndDate > DateTime.UtcNow && auction.IsActive;

            dto.CreatorUserName = auction.User.UserName;

            return dto;
        }

        // -------------------- SEARCH OPEN --------------------

        public async Task<List<AuctionResponseDto>> SearchOpenAsync(string title)
        {
            var auctions = await _auctionRepo.SearchByTitleAsync(title);

            var filtered = auctions
                .Where(a => a.EndDate > DateTime.UtcNow && a.IsActive)
                .ToList();

            return await GetMappedAuctions(filtered);
        }

        // -------------------- CREATE --------------------

        public async Task<AuctionResponseDto> CreateAsync(CreateAuctionDto dto, int userId)
        {
            if (dto.StartPrice <= 0)
                throw new Exception("Start price must be greater than zero");

            var startDate = DateTime.UtcNow;

            var auction = _mapper.Map<Auction>(dto);

            auction.UserId = userId;
            auction.StartDate = startDate;
            auction.EndDate = startDate.AddDays(AuctionDurationDays);
            auction.IsActive = true;

            await _auctionRepo.AddAsync(auction);

            var createdAuction = await _auctionRepo.GetByIdWithUserAsync(auction.AuctionId);

            var response = _mapper.Map<AuctionResponseDto>(createdAuction);

            response.CreatorUserName = createdAuction.User.UserName;
            response.HighestBid = createdAuction.StartPrice;
            response.IsOpen = createdAuction.EndDate > DateTime.UtcNow && createdAuction.IsActive;

            return response;
        }


        // -------------------- UPDATE --------------------

        public async Task<bool> UpdateAsync(UpdateAuctionDto dto, int userId)
        {
            var auction = await _auctionRepo.GetByIdAsync(dto.AuctionId);

            if (auction == null)
                return false;

            // Only owner can update
            if (auction.UserId != userId)
                return false;

            // Cannot update closed auction
            if (auction.EndDate < DateTime.UtcNow)
                return false;

            // Check price change rule
            var hasBids = await _auctionRepo.HasBidsAsync(dto.AuctionId);

            if (hasBids && dto.StartPrice != auction.StartPrice)
                return false;

            auction.Title = dto.Title;
            auction.Description = dto.Description;
            auction.EndDate = dto.EndDate;
            auction.StartPrice = dto.StartPrice;

            await _auctionRepo.UpdateAsync(auction);

            return true;
        }

        // -------------------- DEACTIVATE --------------------

        public async Task<bool> DeactivateAsync(int auctionId)
        {
            var auction = await _auctionRepo.GetByIdAsync(auctionId);

            if (auction == null)
                return false;

            auction.IsActive = false;

            await _auctionRepo.UpdateAsync(auction);

            return true;
        }

        // -------------------- PRIVATE HELPER --------------------

        private async Task<List<AuctionResponseDto>> GetMappedAuctions(List<Auction> auctions)
        {
            var result = auctions.Select(a =>
            {
                var dto = _mapper.Map<AuctionResponseDto>(a);

                dto.HighestBid = a.Bids.Any()
                    ? a.Bids.Max(b => b.Amount)
                    : a.StartPrice;

                dto.IsOpen = a.EndDate > DateTime.UtcNow && a.IsActive;

                dto.CreatorUserName = a.User.UserName;

                return dto;
            }).ToList();

            return result;
        }
    }
}

