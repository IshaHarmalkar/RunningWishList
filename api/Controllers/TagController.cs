using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Tag;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using Humanizer.DateTimeHumanizeStrategy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/tags")]
    [ApiController]
    [Authorize]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepo;
        public TagController(ITagRepository tagRepo)
        {
            _tagRepo = tagRepo;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetUserId();
            var tags = await _tagRepo.GetAllAsync(userId);
            return Ok(tags);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.GetUserId();

            var existing = await _tagRepo.GetByNameAsync(dto.Name, userId);
            if(existing != null)
            {
                return Conflict(new { message = $"tag '{dto.Name}' already exists"});
            }

            var tag = dto.ToModel(userId);
            var created = await _tagRepo.CreateAsync(tag);

            return CreatedAtAction(nameof(GetAll), new TagResponseDto
            {
                Id = created.Id,
                Name = created.Name,
               Count = 0
            });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            var deleted = await _tagRepo.DeleteAsync(id, userId);

            if(!deleted) return NotFound(new { message = "Tag not found"});

            return NoContent();


        }
        
    }
}