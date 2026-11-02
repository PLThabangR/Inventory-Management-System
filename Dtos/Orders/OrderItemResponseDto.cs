namespace LogiTrack.DTOs.Orders;

public class OrderItemResponseDto
{
    public int InventoryItemId { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public int Quantity { get; set; }
}