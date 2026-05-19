using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.WishlistItem;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class WishlistItemRepository : IWishlistItemRepository
    {
        private readonly ApplicationDBContext _context;
        public WishlistItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<WishlistItem> CreateAsync(WishlistItem item, List<string> tagNames)
        {
            foreach(var name in tagNames.Select(t => t.ToLower().Trim()).Distinct())
            {
                var tag = await _context.Tags
                    .FirstOrDefaultAsync(t => t.Name == name && t.UserId == item.UserId);

                if(tag == null)
                {
                    tag = new Tag { Name = name, UserId  = item.UserId};
                    await _context.Tags.AddAsync(tag);
                    await _context.SaveChangesAsync();
                }

                item.WishlistItemTags.Add(new WishlistItemTag
                {
                    TagId = tag.Id,
                    WishlistItemId = item.Id
                });
            }

            await _context.WishlistItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var item = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.Id == id &&  w.UserId == userId );

            if(item == null) return false;

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<WishlistItemPagedResponseDto> GetAllAsync(Guid userId, WishlistItemQueryParams query)
        {
            var queryable = _context.WishlistItems
                .Where(w => w.UserId == userId)
                .Include(w => w.WishlistItemTags)
                    .ThenInclude(wt => wt.Tag)
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Status))
            {
                queryable = queryable.Where(w => w.Status == query.Status.ToUpper());
            }
            if(!string.IsNullOrWhiteSpace(query.Tag))
            {
                queryable = queryable.Where(w => w.WishlistItemTags.Any(wt => wt.Tag.Name == query.Tag.ToLower()));
            }

            if(!string.IsNullOrEmpty(query.Search))
            {
                queryable = queryable.Where(w => w.Title.Contains(query.Search) ||
                w.Domain.Contains(query.Search));
            }

            var totalCount = await queryable.CountAsync();
            var skip = (query.Page - 1) * query.PageSize;

            var items = await queryable
                .OrderByDescending(w => w.CreatedAt)
                .Skip(skip)
                .Take(query.PageSize)
                .Select(w => new WishlistItemResponseDto
                {
                    Id = w.Id,
                    Url = w.Url,
                    Title = w.Title,
                    Price = w.Price,
                    Currency = w.Currency,
                    ImageUrl = w.ImageUrl,
                    Domain = w.Domain,
                    Notes = w.Notes,
                    Status = w.Status,
                    Tags = w.WishlistItemTags.Select(wt => wt.Tag.Name).ToList(),
                    CreatedAt = w.CreatedAt
                }).ToListAsync();

                
            return new WishlistItemPagedResponseDto
            {
                Items = items,
                Pagination = new PaginationDto
                {
                    Page = query.Page,
                    HasMore = skip + items.Count < totalCount
                }
            };
        }

        public async Task<WishlistItem?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.WishlistItems
                .Include(w => w.WishlistItemTags)
                 .ThenInclude(wt => wt.Tag)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
        }

        public async Task<WishlistItem> UpdateAsync(Guid id, Guid userId, WishlistItemUpdateDto dto)
        {
            var item = await _context.WishlistItems
                .Include(w => w.WishlistItemTags)
                    .ThenInclude(wt => wt.Tag)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
                
            if(item == null) return null;

            //patch only provided fields

            if(dto.Title != null) item.Title = dto.Title;
            if(dto.Price.HasValue) item.Price = dto.Price;
            if(dto.Currency != null) item.Currency = dto.Currency;
            if(dto.ImageUrl != null) item.ImageUrl = dto.ImageUrl;
            if(dto.Notes != null) item.Notes = dto.Notes;
            item.UpdatedAt = DateTime.UtcNow;

            if(dto.tags != null)
            {
                _context.WishlistItemTags.RemoveRange(item.WishlistItemTags);

                foreach( var name in dto.tags.Select(t => t.ToLower().Trim()).Distinct())
                {
                    var tag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Name == name && t.UserId == userId);

                    if(tag == null)
                    {
                        tag = new Tag { Name = name, UserId = userId};
                        await _context.Tags.AddAsync(tag);
                        await _context.SaveChangesAsync();
                        //move outside later
                    }

                    item.WishlistItemTags.Add( new WishlistItemTag
                    {
                        TagId = tag.Id,
                        WishlistItemId = item.Id
                    });                   
                }               
            }
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<WishlistItem> UpdateStatusAsync(Guid id, Guid userId, string status)
        {
           var item = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if(item == null) return null;

            item.Status = status;
            item.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return item;
        }
    }
}