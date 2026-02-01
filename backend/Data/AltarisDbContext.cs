using app.webapi.backoffice_viajes_altairis.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace app.webapi.backoffice_viajes_altairis.Data
{
    public class AltarisDbContext(DbContextOptions<AltarisDbContext> options) : DbContext(options)
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
