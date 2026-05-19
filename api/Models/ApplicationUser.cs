using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}