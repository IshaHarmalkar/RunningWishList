using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.WishlistItem;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;

namespace api.Controllers
{
    [Route("api/wishlist-items")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistItemRepository _repo;

        public WishlistController(IWishlistItemRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] WishlistItemQueryParams query)
        {
            var userId = User.GetUserId();
            var result = await _repo.GetAllAsync(userId, query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = User.GetUserId();
            var item = await _repo.GetByIdAsync(id, userId);

            if(item == null) return NotFound(new
            {
                message = "item not found."
            });

            return Ok(item.ToResponseDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create ([FromBody] WishlistItemRequestDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.GetUserId();
            var item = dto.ToModel(userId);
            var created = await _repo.CreateAsync(item, dto.Tags);

            return CreatedAtAction(nameof(GetById), new { id = created.Id}, new
            {
                id = created.Id,
                status = created.Status
            });
        }


        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] WishlistItemUpdateDto dto)
        {
            if(!ModelState.IsValid)  return BadRequest(ModelState);

            var userId = User.GetUserId();
            var updated = await _repo.UpdateAsync(id, userId, dto);

            if(updated == null) return NotFound(new { message = "Item not found."});

            return Ok(updated.ToResponseDto());
        }


        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] WishlistItemStatusDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.GetUserId();
            var updated = await _repo.UpdateStatusAsync(id, userId, dto.Status.ToUpper());

            if(updated == null) return NotFound(new { message = "Item not found."});

            return Ok(new {status = updated.Status});
        }
        

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            var deleted = await _repo.DeleteAsync(id, userId);
            if(!deleted) return NotFound(new {message = "Item not found"});

            return NoContent();
        }
    }
}