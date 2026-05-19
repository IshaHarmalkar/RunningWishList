using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class WishlistItemTag
    {
        public Guid WishlistItemId { get; set; }
        public WishlistItem WishlistItem { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}