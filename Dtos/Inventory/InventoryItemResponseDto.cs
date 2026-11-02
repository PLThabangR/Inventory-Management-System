namespace LogiTrack.DTOs.Inventory;
// This DTO is used to send inventory item data in responses.
public class InventoryItemResponseDto
{
    
    public int ItemId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string Location { get; set; } = string.Empty;
}