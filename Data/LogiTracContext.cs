using LogiTrack.Models;
using LogiTrack.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Data;

//We are Extending the IdentityDbContext to include our custom ApplicationUser class, which allows us to manage user authentication and authorization in our application.
public class LogiTrackContext : IdentityDbContext<ApplicationUser>
{
    public LogiTrackContext(
        DbContextOptions<LogiTrackContext> options)
        : base(options)
    {
    }

    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

//COnfigure the relationships between the Order and OrderItem entities using Fluent API. This method is called when the model is being created and allows us to customize the behavior of the entity framework.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.InventoryItem)
            .WithMany()
            .HasForeignKey(oi => oi.InventoryItemId);
    }
}