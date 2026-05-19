using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Tag : BaseEntity
    {
        public Guid UserId { get; set; }        
        public string Name { get; set; }

        public ApplicationUser User {get; set; }
        public ICollection<WishlistItemTag> WishlistItemTags { get; set; } = new List<WishlistItemTag>();



    }
}