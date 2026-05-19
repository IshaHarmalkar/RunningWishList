using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.WishlistItem;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IWishlistItemRepository
    {
        Task<WishlistItemPagedResponseDto> GetAllAsync(Guid userId, WishlistItemQueryParams query);
        Task<WishlistItem?> GetByIdAsync(Guid id, Guid userId);
        Task<WishlistItem> CreateAsync(WishlistItem item, List<string> tagNames);
        Task<WishlistItem> UpdateAsync(Guid id, Guid userId, WishlistItemUpdateDto dto);
        Task<WishlistItem> UpdateStatusAsync(Guid id, Guid userId, string status);
        Task<bool> DeleteAsync(Guid id, Guid userId);
    }
}