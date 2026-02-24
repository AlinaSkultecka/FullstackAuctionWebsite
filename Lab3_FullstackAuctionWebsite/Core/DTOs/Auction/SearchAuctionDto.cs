namespace Lab3_FullstackAuctionWebsite.Core.DTOs.Auction
{
    public class SearchAuctionDto
    {
            public string? BookTitle { get; set; }
            public string? Author { get; set; }
            public string? Genre { get; set; }
            public bool? IsActive { get; set; }
    }
}
