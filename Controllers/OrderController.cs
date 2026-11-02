

using LogiTrack.Data;
using LogiTrack.DTOs.Orders;
using LogiTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Controllers;

public class OrderController : ControllerBase
{
    private readonly LogiTrackContext _context;

    public OrderController(LogiTrackContext context)
    {
        _context = context;
    }

    //Get all orders
    [HttpGet("api/orders")]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
    {
        // Retrieve all orders from the database, including their associated order items and inventory items, without tracking changes.
        var orders = await _context.Orders.AsNoTracking().Include(o => o.Items).ThenInclude(oi => oi.InventoryItem).ToListAsync();
       
       var response = orders.Select(MapToResponseDto).ToList();

       return Ok(response);

       
    }

    //Get a specific order by its ID
    [HttpGet("api/orders/{orderId}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrderById(int orderId)
    {
        // Retrieve the order with the specified ID from the database, including its associated order items and inventory items, without tracking changes.
        var order = await _context.Orders.AsNoTracking().Include(o => o.Items).ThenInclude(oi => oi.InventoryItem).FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
        {
            return NotFound(new{ message = $"Order with ID {orderId} not found."});
        }

        var response = MapToResponseDto(order);

        return Ok(response);
    }

    //Post a new order
    [HttpPost("api/orders")]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder(CreateOrderDto createOrderDto)
    {
        // Create a new Order object from the provided DTO and save it to the database.
        var order = new Order
        {
            CustomerName = createOrderDto.CustomerName,
            DatePlaced = DateTime.UtcNow,
            Items = createOrderDto.Items.Select(item => new OrderItem
            {
                InventoryItemId = item.InventoryItemId,
                Quantity = item.Quantity
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var response = MapToResponseDto(order);

        return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderId }, response);
    }

    ///Delete an order by its ID
    [HttpDelete("api/orders/{orderId}")]
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


    private static OrderResponseDto MapToResponseDto(Order order)
    {
        return new OrderResponseDto
        {
            OrderId = order.OrderId,
            CustomerName = order.CustomerName,
            DatePlaced = order.DatePlaced,
            // Map the order items to their corresponding response DTOs, including the inventory item names.
            Items = order.Items.Select(item => new OrderItemResponseDto
            {
                InventoryItemId = item.InventoryItemId,
                ItemName = item.InventoryItem.Name,
                Quantity = item.Quantity
            }).ToList()
        };
    }




}