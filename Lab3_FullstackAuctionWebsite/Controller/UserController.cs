using Lab3_FullstackAuctionWebsite.Core.DTOs;
using Lab3_FullstackAuctionWebsite.Core.DTOs.User;
using Lab3_FullstackAuctionWebsite.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab3_FullstackAuctionWebsite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // -------------------- REGISTER --------------------

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            var user = await _userService.RegisterAsync(dto);
            return Ok(user);
        }

        // -------------------- LOGIN --------------------

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            var result = await _userService.LoginAsync(dto);

            if (result == null)
                return Unauthorized("Invalid username or password.");

            return Ok(result);
        }

        // -------------------- GET ALL USERS (ADMIN) --------------------

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // -------------------- UPDATE PASSWORD --------------------

        [Authorize]
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto dto)
        {
            int userId = int.Parse(User.FindFirst("id")!.Value);

            var success = await _userService.UpdatePasswordAsync(userId, dto);

            if (!success)
                return BadRequest("Password update failed.");

            return Ok("Password updated successfully.");
        }

        // -------------------- DEACTIVATE USER (ADMIN) --------------------

        [Authorize(Roles = "Admin")]
        [HttpPut("deactivate/{userId}")]
        public async Task<IActionResult> Deactivate(int userId)
        {
            var success = await _userService.DeactivateAsync(userId);

            if (!success)
                return NotFound();

            return Ok("User deactivated.");
        }
    }
}
