using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace api.Dtos.WishlistItem
{
    public class WishlistItemResponseDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public string? ImageUrl { get; set; }
        public string? Domain { get; set; }
        public string? Notes { get; set; }

        public string Status { get; set; }
        public List<string> Tags { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}