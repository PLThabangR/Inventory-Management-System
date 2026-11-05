using LogiTrack.DTOs.Inventory;
using LogiTrack.DTOs.Orders;
using LogiTrack.Models;
using Mapster;

namespace LogiTrack.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        // Inventory mappings
        TypeAdapterConfig<InventoryItem, InventoryItemResponseDto>
            .NewConfig();

        TypeAdapterConfig<CreateInventoryItemDto, InventoryItem>
            .NewConfig();


        // Order mappings
        TypeAdapterConfig<Order, OrderResponseDto>
            .NewConfig();

        TypeAdapterConfig<CreateOrderDto, Order>
            .NewConfig();

//We have relationship here of inventory in the order item
        TypeAdapterConfig<OrderItem, OrderItemResponseDto>
            .NewConfig()
            .Map(dest => dest.ItemName,
                 src => src.InventoryItem.Name);

        TypeAdapterConfig<CreateOrderItemDto, OrderItem>
            .NewConfig();


            
    }
}