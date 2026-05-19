using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace api.Models
{
    public class WishlistItem : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        public string Currency { get; set;} = "INR";
        public string ImageUrl { get; set; }
        public string Domain { get; set; }
        public string Notes { get; set; }

        public string Status { get; set; } = "ACTIVE";
        //ACTIVE, ARCHIVED, BOUGHT

        public ApplicationUser User { get; set; }
        public ICollection<WishlistItemTag> WishlistItemTags { get; set; } = new List<WishlistItemTag>();


        
    }
}