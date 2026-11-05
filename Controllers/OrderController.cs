

using LogiTrack.Data;
using LogiTrack.DTOs.Orders;
using LogiTrack.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Controllers;

[Authorize]
[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly LogiTrackContext _context;

    public OrderController(LogiTrackContext context)
    {
        _context = context;
    }

    //Get all orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
    {
        // Retrieve all orders from the database, including their associated order items and inventory items, without tracking changes.
        var orders = await _context.Orders.AsNoTracking().Include(o => o.Items).ThenInclude(oi => oi.InventoryItem).ToListAsync();
       
       var response = orders.Adapt<List<OrderResponseDto>>();

       return Ok(response);

       
    }

    //Get a specific order by its ID
    [HttpGet("{orderId:int}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrderById(int orderId)
    {
        // Retrieve the order with the specified ID from the database, including its associated order items and inventory items, without tracking changes.
        var order = await _context.Orders.AsNoTracking().Include(o => o.Items).ThenInclude(oi => oi.InventoryItem).FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
        {
            return NotFound(new{ message = $"Order with ID {orderId} not found."});
        }

        var response = order.Adapt<OrderResponseDto>();

        return Ok(response);
    }

    //Post a new order
    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder(
    CreateOrderDto createOrderDto)
    {
        // 1. Prevent null or empty orders.
        if (createOrderDto.Items == null ||
            !createOrderDto.Items.Any())
        {
            return BadRequest(new
            {
                message = "An order must contain at least one item."
            });
        }

        // 2. Validate individual item quantities and IDs.
        if (createOrderDto.Items.Any(i =>
            i.InventoryItemId <= 0 || i.Quantity <= 0))
        {
            return BadRequest(new
            {
                message = "Every inventory item must have a valid ID and a quantity greater than zero."
            });
        }

        // 3. Combine duplicate inventory items.
        var requestedItems = createOrderDto.Items
            .GroupBy(i => i.InventoryItemId)
            .Select(g => new
            {
                InventoryItemId = g.Key,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();

        // 4. Get unique inventory IDs.
        var inventoryItemIds = requestedItems
            .Select(i => i.InventoryItemId)
            .ToList();

        // 5. Retrieve inventory records from the database.
        // AsTracking ensures EF Core tracks stock modifications.
        var inventoryItems = await _context.InventoryItems
            .AsTracking()
            .Where(i => inventoryItemIds.Contains(i.ItemId))
            .ToDictionaryAsync(i => i.ItemId);

        // 6. Check whether all requested inventory items exist.
        var missingItemIds = inventoryItemIds
            .Except(inventoryItems.Keys)
            .ToList();

        if (missingItemIds.Any())
        {
            return BadRequest(new
            {
                message = "Some inventory items do not exist.",
                missingInventoryItemIds = missingItemIds
            });
        }

        // 7. Check whether sufficient stock is available.
        var insufficientStock = requestedItems
            .Where(requested =>
                inventoryItems[requested.InventoryItemId].Quantity
                < requested.Quantity)
            .Select(requested => new
            {
                ItemId = requested.InventoryItemId,
                ItemName = inventoryItems[requested.InventoryItemId].Name,
                RequestedQuantity = requested.Quantity,
                AvailableQuantity = inventoryItems[requested.InventoryItemId].Quantity
            })
            .ToList();

        if (insufficientStock.Any())
        {
            return Conflict(new
            {
                message = "Insufficient inventory available to fulfill this order.",
                items = insufficientStock
            });
        }

        // 8. Map the request DTO to the Order entity using Mapster.
        var order = createOrderDto.Adapt<Order>();

        order.DatePlaced = DateTime.UtcNow;

        // 9. Create order lines using validated inventory IDs.
        order.Items = requestedItems
            .Select(item => new OrderItem
            {
                InventoryItemId = item.InventoryItemId,
                Quantity = item.Quantity
            })
            .ToList();

        // 10. Deduct stock from inventory.
        foreach (var requested in requestedItems)
        {
            //Get the Id of the Item from inventory and deduct quantity
            var inventoryItem =inventoryItems[requested.InventoryItemId];

            inventoryItem.Quantity -= requested.Quantity;
        }

        // 11. Save the order and inventory changes together.
        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        // 12. Retrieve the saved order with its inventory details.
        var savedOrder = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(i => i.InventoryItem)
            .FirstAsync(o => o.OrderId == order.OrderId);

        // 13. Map the result using Mapster.
        var responseDto = savedOrder.Adapt<OrderResponseDto>();

        return CreatedAtAction(
            nameof(GetOrderById),
            new { orderId = order.OrderId },
            responseDto);
    }

    [Authorize(Roles = "Manager")]
    ///Delete an order by its ID
    [HttpDelete("{orderId:int}")]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        // Retrieve the order with the specified ID from the database.
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null)
        {
            return NotFound(new { message = $"Order with ID {orderId} not found." });
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }






}