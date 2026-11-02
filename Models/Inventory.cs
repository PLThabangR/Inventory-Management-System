using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Models;

public class InventoryItem
{
    [Key]
    public int ItemId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [MaxLength(100)]
    public string Location { get; set; } = string.Empty;

    public void DisplayInfo()
    {
        Console.WriteLine(
            $"Item: {Name} | Quantity: {Quantity} | Location: {Location}");
    }
}