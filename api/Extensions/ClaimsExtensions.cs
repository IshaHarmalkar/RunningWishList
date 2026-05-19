using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;

namespace api.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User Id claim not found.");
        
            return Guid.Parse(claim.Value);
        }
    }
}