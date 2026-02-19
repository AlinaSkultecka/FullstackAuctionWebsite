using Lab3_FullstackAuctionWebsite.Core.DTOs.User;

namespace Lab3_FullstackAuctionWebsite.Core.Interfaces
{
     public interface IUserService
     {
         Task<UserResponseDto> RegisterAsync(RegisterUserDto dto);

         Task<LoginResponseDto?> LoginAsync(LoginUserDto dto);

         Task<List<UserResponseDto>> GetAllAsync();

         Task<bool> UpdatePasswordAsync(int userId, UpdatePasswordDto dto);

         Task<bool> DeactivateAsync(int userId);
     }
}