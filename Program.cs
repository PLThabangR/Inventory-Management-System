using LogiTrack.Data;
using LogiTrack.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//DB context 
builder.Services.AddDbContext<LogiTrackContext>(options =>
    options.UseSqlite("Data Source=logitrack.db"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

app.Run();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<LogiTrackContext>();

    context.Database.EnsureCreated();

    if (!context.InventoryItems.Any())
    {
        var palletJack = new InventoryItem
        {
            Name = "Pallet Jack",
            Quantity = 12,
            Location = "Warehouse A"
        };

        var forklift = new InventoryItem
        {
            Name = "Forklift",
            Quantity = 5,
            Location = "Warehouse B"
        };

        context.InventoryItems.AddRange(palletJack, forklift);

        context.SaveChanges();
    }

    var inventory = context.InventoryItems.ToList();

    Console.WriteLine("\n--- INVENTORY ---");

    foreach (var item in inventory)
    {
        item.DisplayInfo();
    }

    var order = new Order
    {
        CustomerName = "Samir",
        DatePlaced = DateTime.Now
    };

    order.AddItem(inventory[0]);
    order.AddItem(inventory[1]);

    context.Orders.Add(order);
    context.SaveChanges();

    Console.WriteLine("\n--- ORDER ---");

    var savedOrder = context.Orders
        .Include(o => o.Items)
        .First(o => o.OrderId == order.OrderId);

    Console.WriteLine(savedOrder.GetOrderSummary());
}