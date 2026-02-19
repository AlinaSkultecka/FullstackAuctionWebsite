namespace Lab3_FullstackAuctionWebsite.Core.DTOs.User
{
    public class UserResponseDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }
    }
}
