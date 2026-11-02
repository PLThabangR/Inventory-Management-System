using LogiTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Data;

public class LogiTrackContext : DbContext
{
    public LogiTrackContext(
        DbContextOptions<LogiTrackContext> options)
        : base(options)
    {
    }

    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

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