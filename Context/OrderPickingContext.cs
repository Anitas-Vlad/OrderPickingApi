using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Context;

public class OrderPickingContext : DbContext
{
    public OrderPickingContext(DbContextOptions<OrderPickingContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(user => user.Roles)
            .HasConversion<string>(); // Converts the enum to a string in the database

        base.OnModelCreating(modelBuilder);
        ModelBuilderExtensions.Seed(modelBuilder);
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<PickingOrder> PickingOrders { get; set; } = default!;
    public DbSet<ReachingOrder> ReachingOrders { get; set; } = default!;
    public DbSet<RelocatingOrder> RelocatingOrders { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<Container> Containers { get; set; } = default!;
    public DbSet<Item> Items { get; set; } = default!;
    public DbSet<Location> Locations { get; set; } = default!;
    public DbSet<Pick> Picks { get; set; } = default!;
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
                    Roles = { UserRole.Picker },
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Avlad1..")
                },
                new User
                {
                    Id = 1001,
                    Username = "Malaka",
                    Roles = { UserRole.Picker },
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Malaka1..")
                },
                new User
                {
                    Id = 1002,
                    Username = "Tomas",
                    Roles = { UserRole.Reacher, UserRole.Picker },
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tomas1..")
                },
                new User
                {
                    Id = 1004,
                    Username = "Admin",
                    Roles = { UserRole.Admin },
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Owner123.")
                }
            );
        }
    }
}