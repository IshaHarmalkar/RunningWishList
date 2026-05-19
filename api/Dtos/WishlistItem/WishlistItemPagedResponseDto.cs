using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.WishlistItem
{
    public class WishlistItemPagedResponseDto
    {
        public List<WishlistItemResponseDto> Items { get; set;} = new();
        public PaginationDto Pagination { get; set; }
    }

    public class PaginationDto
    {
        public int Page { get; set; }
        public bool HasMore { get; set; }
    }
}