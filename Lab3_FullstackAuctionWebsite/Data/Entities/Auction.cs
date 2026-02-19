using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab3_FullstackAuctionWebsite.Data.Entities
{
    public class Auction
    {
        public int AuctionId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal StartPrice { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // VG requirement
        public bool IsActive { get; set; } = true;

        // Foreign Key
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation
        public List<Bid> Bids { get; set; } = new();
    }
}

