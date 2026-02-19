using Lab3_FullstackAuctionWebsite.Core.DTOs;
using Lab3_FullstackAuctionWebsite.Core.DTOs.Auction;
using Lab3_FullstackAuctionWebsite.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab3_FullstackAuctionWebsite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        // -------------------- GET ALL --------------------

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var auctions = await _auctionService.GetAllAsync();
            return Ok(auctions);
        }

        // -------------------- GET BY ID --------------------

        [HttpGet("{auctionId}")]
        public async Task<IActionResult> GetById(int auctionId)
        {
            var auction = await _auctionService.GetByIdAsync(auctionId);

            if (auction == null)
                return NotFound();

            return Ok(auction);
        }

        // -------------------- SEARCH OPEN --------------------

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string title)
        {
            var result = await _auctionService.SearchOpenAsync(title);
            return Ok(result);
        }

        // -------------------- CREATE --------------------

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAuctionDto dto)
        {
            int userId = int.Parse(User.FindFirst("id")!.Value);

            var created = await _auctionService.CreateAsync(dto, userId);

            return Ok(created);
        }

        // -------------------- UPDATE --------------------

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAuctionDto dto)
        {
            int userId = int.Parse(User.FindFirst("id")!.Value);

            var success = await _auctionService.UpdateAsync(dto, userId);

            if (!success)
                return BadRequest("Update failed.");

            return Ok("Auction updated successfully.");
        }

        // -------------------- DEACTIVATE (ADMIN) --------------------

        [Authorize(Roles = "Admin")]
        [HttpPut("deactivate/{auctionId}")]
        public async Task<IActionResult> Deactivate(int auctionId)
        {
            var success = await _auctionService.DeactivateAsync(auctionId);

            if (!success)
                return NotFound();

            return Ok("Auction deactivated.");
        }
    }
}

