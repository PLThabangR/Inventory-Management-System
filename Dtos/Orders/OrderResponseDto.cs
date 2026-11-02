namespace LogiTrack.DTOs.Orders;

public class OrderResponseDto
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public DateTime DatePlaced { get; set; }

    public List<OrderItemResponseDto> Items { get; set; } = [];
}