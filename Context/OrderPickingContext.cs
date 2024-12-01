using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Models.TaskRequests;

namespace OrderPickingSystem.Context;

public class OrderPickingContext : DbContext
{
    public OrderPickingContext(DbContextOptions<OrderPickingContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // ModelBuilderExtensions.Seed(modelBuilder);
        // Configure PickingOrder to PickRequest relationship (for RequestedItems)
        modelBuilder.Entity<PickRequest>()
            .HasOne<PickingOrder>() // Specify PickingOrder as the parent
            .WithMany(po => po.RequestedItems) // Navigation for RequestedItems
            .HasForeignKey(pr => pr.OrderId) // Foreign key column
            .OnDelete(DeleteBehavior.Cascade);

        // Configure PickingOrder to PickRequest relationship (for ReplenishedRequestedItems)
        modelBuilder.Entity<PickRequest>()
            .HasOne<PickingOrder>() // Specify PickingOrder as the parent
            .WithMany(po => po.ReplenishedRequestedItems) // Navigation for ReplenishedRequestedItems
            .HasForeignKey(pr => pr.OrderId) // Foreign key column
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PickingOrder>()
            .Ignore(o => o.RequestedItems); // Explicitly ignore inferred navigation

        modelBuilder.Entity<PickingOrder>()
            .Ignore(o => o.ReplenishedRequestedItems); // Explicitly ignore inferred navigation

        modelBuilder.Entity<Palette>()
            .HasOne(p => p.OngoingContainer) // Specify the navigation property
            .WithMany() // No inverse navigation on Container
            .HasForeignKey(p => p.OngoingContainerId) // Specify the foreign key property
            .OnDelete(DeleteBehavior.Restrict); // Optional: Set delete behavior

        modelBuilder.Entity<UserRoleMapping>()
            .HasKey(urm => new { urm.UserId, urm.Role });

        // Configure the relationship between PickingOrder and RequestedItems
        modelBuilder.Entity<PickingOrder>()
            .HasMany(p => p.RequestedItems)
            .WithOne()
            .HasForeignKey(pr => pr.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed

        // Configure the relationship between PickingOrder and ReplenishedRequestedItems
        modelBuilder.Entity<PickingOrder>()
            .HasMany(p => p.ReplenishedRequestedItems)
            .WithOne()
            .HasForeignKey(pr => pr.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed

        ModelBuilderExtensions.Seed(modelBuilder);
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<UserRoleMapping> UserRoleMappings { get; set; } = default!;
    public DbSet<PickingOrder> PickingOrders { get; set; } = default!;
    public DbSet<ReachingOrder> ReachingOrders { get; set; } = default!;
    public DbSet<RelocatingOrder> RelocatingOrders { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<Container> Containers { get; set; } = default!;
    public DbSet<Item> Items { get; set; } = default!;
    public DbSet<Location> Locations { get; set; } = default!;
    public DbSet<Pick> Picks { get; set; } = default!;
    public DbSet<Reach> Reaches { get; set; } = default!;
    public DbSet<Relocation> Relocations { get; set; } = default!;
    public DbSet<Palette> Palettes { get; set; } = default!;


    private static class ModelBuilderExtensions
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1000,
                    Username = "Vlad",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Vlad123..")
                },
                new User
                {
                    Id = 1001,
                    Username = "Alex",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Alex123..")
                }
            );
        
            modelBuilder.Entity<UserRoleMapping>().HasData(
                new UserRoleMapping
                {
                    UserId = 1000,
                    Role = UserRole.Picker
                },
                new UserRoleMapping
                {
                    UserId = 1000,
                    Role = UserRole.Reacher
                },
                new UserRoleMapping
                {
                    UserId = 1001,
                    Role = UserRole.Picker
                }
            );
        }
    }
}