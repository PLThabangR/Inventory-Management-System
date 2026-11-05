using System.ComponentModel.DataAnnotations;

namespace LogiTrack.DTOs.Orders;

public class CreateOrderDto
{
    [Required]
    [MaxLength(150)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "An order must contain at least one item.")]
    public List<CreateOrderItemDto> Items { get; set; } = [];
}