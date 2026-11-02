using System.ComponentModel.DataAnnotations;

namespace LogiTrack.DTOs.Orders;

public class CreateOrderDto
{
    [Required]
    [MaxLength(150)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<CreateOrderItemDto> Items { get; set; } = [];
}