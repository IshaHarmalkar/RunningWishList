using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Tag;
using api.Models;

namespace api.Interfaces
{
    public interface ITagRepository
    {
        Task<List<TagResponseDto>> GetAllAsync(Guid userId);
        Task<Tag?> GetByIdAsync(Guid id, Guid userId);
        Task<Tag?> GetByNameAsync(string name, Guid userId);
        Task<Tag> CreateAsync(Tag tag);
        Task<bool> DeleteAsync(Guid id, Guid userId);

     }
}