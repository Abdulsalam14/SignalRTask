using Microsoft.EntityFrameworkCore;
using WebApiSignalR.Entities;

namespace WebApiSignalR.Data
{
    public class RoomDbContext : DbContext
    {
        public RoomDbContext(DbContextOptions<RoomDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    Name = "Mercedes",
                    Price = 5000
                },
                new Room
                {
                    Id = 2,
                    Name = "Chevrolet",
                    Price = 4000
                },
                new Room
                {
                    Id = 3,
                    Name = "Maseratti",
                    Price = 7000
                });
        }
    }
}
