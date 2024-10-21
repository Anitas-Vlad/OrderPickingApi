using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Models;

namespace OrderPickingSystem.Context;

public class OrderPickingContext : DbContext
{
    public OrderPickingContext(DbContextOptions<OrderPickingContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelBuilderExtensions.Seed(modelBuilder);
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<Container> Containers { get; set; } = default!;
    public DbSet<Item> Items { get; set; } = default!;
    public DbSet<LocationItem> LocationItems { get; set; } = default!;
    public DbSet<Location> Locations { get; set; } = default!;
    public DbSet<Pick> Picks { get; set; } = default!;


    private static class ModelBuilderExtensions
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1000,
                    Username = "User 1",
                    Email = "user1@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User11..")
                },
                new User
                {
                    Id = 1001,
                    Username = "User 2",
                    Email = "user2@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User22..")
                },
                new User
                {
                    Id = 1002,
                    Username = "User 3",
                    Email = "user3@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User33..")
                },
                new User
                {
                    Id = 1004,
                    Username = "Owner",
                    Email = "owner@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Owner123.")
                }
            );
        }
    }
}