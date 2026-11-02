// This is the InventoryController class, which is responsible for handling HTTP requests related to inventory management in the LogiTrack application.
using LogiTrack.Data;
using LogiTrack.DTOs.Inventory;
using LogiTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Controller;

public class InventoryController:ControllerBase
{
    //private readonly LogiTrackContext _context cannot be null because it is initialized in the constructor.
    private readonly LogiTrackContext _context;

    // Constructor for the InventoryController class, which initializes the controller with a database context.
    public InventoryController(LogiTrackContext context)
    {
        _context = context;
    }
    // GET: api/inventory
    [HttpGet("api/inventory")]
    public async Task<ActionResult<IEnumerable<InventoryItemResponseDto>>> GetAllInventoryItems()
    {
       var inventoryItems = await _context.InventoryItems.AsNoTracking().ToListAsync();
        var inventoryItemList = inventoryItems.Select(item => new InventoryItemResponseDto
        {
            ItemId = item.ItemId,
            Name = item.Name,
            Quantity = item.Quantity,
            Location = item.Location
        }).ToList();

        return Ok(inventoryItemList);
    }

    // POST: api/inventory
    [HttpPost("api/inventory")]
    public async Task<ActionResult<InventoryItemResponseDto>> CreateInventoryItem(CreateInventoryItemDto createDto)
    {
        //Create a new InventoryItem object from the provided DTO and save it to the database.
        var inventoryItem = new InventoryItem
        {
            Name = createDto.Name,
            Quantity = createDto.Quantity,
            Location = createDto.Location
        };
        //Add to the database and save changes asynchronously.
        _context.InventoryItems.Add(inventoryItem);
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
    [HttpGet("api/inventory/{id}")]
    public async Task<ActionResult<InventoryItemResponseDto>> GetInventoryItemById(int id)
    {
        var inventoryItem = await _context.InventoryItems.FirstOrDefaultAsync(x => x.ItemId == id);

        if (inventoryItem == null)
        {
            return NotFound();
        }

        var responseDto = new InventoryItemResponseDto
        {
            ItemId = inventoryItem.ItemId,
            Name = inventoryItem.Name,
            Quantity = inventoryItem.Quantity,
            Location = inventoryItem.Location
        };

        return Ok(responseDto);
    }

    //Delete: api/inventory/{id}
    [HttpDelete("api/inventory/{id}")]
    public async Task<IActionResult> DeleteInventoryItem(int id)
    {
        var inventoryItem = await _context.InventoryItems.FirstOrDefaultAsync(x => x.ItemId == id);

        if (inventoryItem == null)
        {
            return NotFound();
        }
            //Remove the inventory item from the database and save changes asynchronously.  
        _context.InventoryItems.Remove(inventoryItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
}//End of Controller