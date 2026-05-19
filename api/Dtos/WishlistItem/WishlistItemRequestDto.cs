using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;

namespace api.Dtos.WishlistItem
{
    public class WishlistItemRequestDto
    {
        [Required]
        [Url]
        public string Url { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public string? Notes {get; set;}

        public List<string> Tags { get; set; } = new();


    }
}