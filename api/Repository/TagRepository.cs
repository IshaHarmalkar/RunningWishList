using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Tag;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDBContext _context;
        public TagRepository(ApplicationDBContext context)
        {
            _context = context;
            
        }

        public async Task<Tag> CreateAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return tag;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (tag == null) return false;

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TagResponseDto>> GetAllAsync(Guid userId)
        {
            return await _context.Tags
                .Where(t => t.UserId == userId)
                .Select(t => new TagResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Count = t.WishlistItemTags.Count
                })
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<Tag?> GetByNameAsync(string name, Guid userId)
        {
            return await _context.Tags
                .FirstOrDefaultAsync( t => t.Name == name.ToLower() && t.UserId == userId); 
        }
    }
}