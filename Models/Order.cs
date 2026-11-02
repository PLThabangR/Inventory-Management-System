using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    [MaxLength(150)]
    public string CustomerName { get; set; } = string.Empty;

    public DateTime DatePlaced { get; set; }

// Navigation property for the related OrderItems
    public ICollection<OrderItem> Items { get; set; }
        = new List<OrderItem>();

    public void AddItem(InventoryItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var existingItem = Items.FirstOrDefault(
            x => x.InventoryItemId == item.ItemId);

        if (existingItem != null)
        {
            existingItem.Quantity++;
            return;
        }

        Items.Add(new OrderItem
        {
            InventoryItem = item,
            InventoryItemId = item.ItemId,
            Quantity = 1
        });
    }

    public void RemoveItem(int itemId)
    {
        var orderItem = Items.FirstOrDefault(
            x => x.InventoryItemId == itemId);

        if (orderItem != null)
        {
            Items.Remove(orderItem);
        }
    }

    public string GetOrderSummary()
    {
        return $"Order #{OrderId} for {CustomerName} | " +
               $"Items: {Items.Sum(x => x.Quantity)} | " +
               $"Placed: {DatePlaced.ToShortDateString()}";
    }
}