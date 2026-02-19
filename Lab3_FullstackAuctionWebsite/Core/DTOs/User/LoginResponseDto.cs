namespace Lab3_FullstackAuctionWebsite.Core.DTOs.User
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }
    }
}
