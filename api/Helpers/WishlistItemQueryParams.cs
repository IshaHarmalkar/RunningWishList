using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class WishlistItemQueryParams
    {
        private const int MaxPageSize = 20;
        private int _pageSize = 10;

        public string? Status { get; set;}
        public string? Tag { get; set;}
        public string? Search {get; set; }
        public int Page { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}