using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Lab3_FullstackAuctionWebsite.Core.DTOs;
using Lab3_FullstackAuctionWebsite.Core.DTOs.User;
using Lab3_FullstackAuctionWebsite.Core.Interfaces;
using Lab3_FullstackAuctionWebsite.Data.Entities;
using Lab3_FullstackAuctionWebsite.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab3_FullstackAuctionWebsite.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(
            IUserRepo userRepo,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        // -------------------- REGISTER --------------------

        public async Task<UserResponseDto> RegisterAsync(RegisterUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            user.IsActive = true;
            user.IsAdmin = false;

            await _userRepo.AddUserAsync(user);

            return _mapper.Map<UserResponseDto>(user);
        }

        // -------------------- LOGIN --------------------

        public async Task<LoginResponseDto?> LoginAsync(LoginUserDto dto)
        {
            var user = await _userRepo.GetByUserNameAsync(dto.UserName);

            if (user == null)
                return null;

            if (!user.IsActive)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
                return null;

            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = token,
                UserId = user.UserId,
                UserName = user.UserName,
                IsAdmin = user.IsAdmin
            };
        }

        // -------------------- GET ALL USERS --------------------

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return users.Select(u => _mapper.Map<UserResponseDto>(u)).ToList();
        }

        // -------------------- UPDATE PASSWORD --------------------

        public async Task<bool> UpdatePasswordAsync(int userId, UpdatePasswordDto dto)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                return false;

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.CurrentPassword
            );

            if (result == PasswordVerificationResult.Failed)
                return false;

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword); ;

            await _userRepo.UpdateUserAsync(user);

            return true;
        }

        // -------------------- DEACTIVATE USER --------------------

        public async Task<bool> DeactivateAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                return false;

            user.IsActive = false;

            await _userRepo.UpdateUserAsync(user);

            return true;
        }

        // -------------------- JWT GENERATION --------------------

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
                new Claim("id", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
