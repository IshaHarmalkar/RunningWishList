using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.WishlistItem
{
    public class WishlistItemStatusDto
    {
        [Required]
        [RegularExpression("^(?i)(ACTIVE|ARCHIVED|BOUGHT)$",
          ErrorMessage = "Status must be ACTIVE,ARCHIVED OR BOUGHT."
        )]
        public string Status { get; set; }
    }
}