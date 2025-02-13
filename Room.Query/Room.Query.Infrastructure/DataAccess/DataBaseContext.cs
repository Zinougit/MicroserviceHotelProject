using Microsoft.EntityFrameworkCore;
using Room.Query.Domain.Entities;

namespace Room.Query.Infrastructure.DataAccess
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options) { }
        public DbSet<RoomEntity> Rooms { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomEntity>(entity =>
            {
                entity.OwnsOne(r => r.Amenities); 
            });
        }
    }
}
