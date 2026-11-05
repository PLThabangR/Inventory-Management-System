// This is the InventoryController class, which is responsible for handling HTTP requests related to inventory management in the LogiTrack application.
using LogiTrack.Data;
using LogiTrack.DTOs.Inventory;
using LogiTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
namespace LogiTrack.Controllers;

[Authorize]
[ApiController]
[Route("api/inventory")]
public class InventoryController:ControllerBase
{
    //private readonly LogiTrackContext _context cannot be null because it is initialized in the constructor.
    private readonly LogiTrackContext _context;
    //inject Caching
    private readonly IMemoryCache _cache;

    // Constructor for the InventoryController class, which initializes the controller with a database context.
    public InventoryController(LogiTrackContext context,IMemoryCache cache)
    {
        _context = context;
        _cache =cache;
    }
    // GET: api/inventory
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemResponseDto>>> GetAllInventoryItems()
    { //Cache the inventory list
       

        var inventoryItemList = await _context.InventoryItems
     .AsNoTracking()
     .ToListAsync();

     // Map the inventory items to response DTOs using mapster
     var responseDtos = inventoryItemList.Adapt<List<InventoryItemResponseDto>>();

        return Ok(responseDtos);
    }

    // POST: api/inventory
    [HttpPost]
    public async Task<ActionResult<InventoryItemResponseDto>> CreateInventoryItem(CreateInventoryItemDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //Create a new InventoryItem object from the provided DTO and save it to the database.
        var inventoryItem = new InventoryItem
        {
            Name = createDto.Name,
            Quantity = createDto.Quantity,
            Location = createDto.Location
        };
        //Add to the database and save changes asynchronously.
      await  _context.InventoryItems.AddAsync(inventoryItem);
        await _context.SaveChangesAsync();

    //Create a response DTO to return the newly created inventory item. 
        var responseDto = new InventoryItemResponseDto
        {
            ItemId = inventoryItem.ItemId,
            Name = inventoryItem.Name,
            Quantity = inventoryItem.Quantity,
            Location = inventoryItem.Location
        };

        return Created("api/inventory", responseDto);

    }

    // GET: api/inventory/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<InventoryItemResponseDto>> GetInventoryItemById(int id)
    {
        var inventoryItem = await _context.InventoryItems.FirstOrDefaultAsync(x => x.ItemId == id);

        if (inventoryItem == null)
        {
            return NotFound(new { message = $"Inventory item with ID {id} not found." });
        }

       // Map the inventory item to a response DTO using mapster
        var responseDto = inventoryItem.Adapt<InventoryItemResponseDto>();

        return Ok(responseDto);
    }

    [Authorize(Roles = "Manager")]
    //Delete: api/inventory/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteInventoryItem(int id)
    {
        var deletedRows = await _context.InventoryItems
     .Where(x => x.ItemId == id)
     .ExecuteDeleteAsync();

        if (deletedRows == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
    
}//End of Controller