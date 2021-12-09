using Catalogo.Dtos;
using Catalogo.Entities;
using Catalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Controllers
{
    [Route("items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _repository;

        public ItemsController(IItemsRepository repository)
        {
            this._repository = repository;
        }

        // GET /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            return (await _repository.GetItemsAsync()).Select(item => item.AsDto());
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await _repository.GetItemAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _repository.CreateItemAsync(item);
            return CreatedAtAction("GetItem", new {id = item.Id}, item.AsDto());
        }
        
        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existinItem = await _repository.GetItemAsync(id);

            if (existinItem is null)
            {
                return NotFound();
            }

            Item updatedItem = existinItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            await _repository.UpdateItemAsync(updatedItem);

            return NoContent();
        }
        
        // Delete /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            var existinItem = await _repository.GetItemAsync(id);

            if (existinItem is null)
            {
                return NotFound();
            }
            
            await _repository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
