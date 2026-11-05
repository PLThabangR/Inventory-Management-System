using System.ComponentModel.DataAnnotations;

namespace LogiTrack.DTOs.Orders;

public class CreateOrderItemDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Inventory item ID must be greater than zero.")]
    public int InventoryItemId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }
}