

using api.Dtos.WishlistItem;
using api.Models;

namespace api.Mappers
{
    public  static class WishlistItemMapper
    {
        public static WishlistItem ToModel(this WishlistItemRequestDto dto, Guid userId)
        {
            return new WishlistItem
            {
                UserId = userId,
                Url = dto.Url,
                Title = dto.Title,
                Price = dto.Price,
                Currency = dto.Currency,
                ImageUrl = dto.ImageUrl,
                Notes = dto.Notes,
                Domain = ExtractDomain(dto.Url),
                Status = "ACTIVE"
            };
        }

        public static WishlistItemResponseDto ToResponseDto(this WishlistItem item)
        {
            return new WishlistItemResponseDto
            {
                Id = item.Id,
                Url = item.Url,
                Title = item.Title,
                Price = item.Price,
                Currency = item.Currency,
                ImageUrl = item.ImageUrl,
                Domain = item.Domain,
                Notes = item.Notes,
                Status = item.Status,
                Tags = item.WishlistItemTags?
                    .Select(wt => wt.Tag.Name)
                    .ToList() ?? new(),
                CreatedAt = item.CreatedAt
            };
        }

        private static string? ExtractDomain(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                ? uri.Host.Replace("www.", "")
                :null;
        }
    }
}