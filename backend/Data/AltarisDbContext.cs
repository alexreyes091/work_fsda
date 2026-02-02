using app.webapi.backoffice_viajes_altairis.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace app.webapi.backoffice_viajes_altairis.Data
{
    public class AltarisDbContext(DbContextOptions<AltarisDbContext> options) : DbContext(options)
    {
        static AltarisDbContext() => AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
       
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<RoomOccupancy> RoomOccupancies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomOccupancy>()
                .HasIndex(o => new { o.RoomId, o.Date })
                .IsUnique();
        }
    }
}
