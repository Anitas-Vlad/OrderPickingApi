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
        ModelBuilderExtensions.Seed(modelBuilder);
    }

    public DbSet<User> Users { get; set; } = default!;
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
                    UserRights = UserRights.Worker,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Avlad1..")
                },
                new User
                {
                    Id = 1001,
                    Username = "Malaka",
                    UserRights = UserRights.Worker,
                    
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Malaka1..")
                },
                new User
                {
                    Id = 1002,
                    Username = "Tomas",
                    UserRights = UserRights.Worker,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tomas1..")
                },
                new User
                {
                    Id = 1004,
                    Username = "Admin",
                    UserRights = UserRights.Admin,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Owner123.")
                }
            );
        }
    }
}