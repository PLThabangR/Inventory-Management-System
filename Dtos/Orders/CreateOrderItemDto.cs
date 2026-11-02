using System.ComponentModel.DataAnnotations;

namespace LogiTrack.DTOs.Orders;

public class CreateOrderItemDto
{
    [Range(1, int.MaxValue)]
    public int InventoryItemId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}