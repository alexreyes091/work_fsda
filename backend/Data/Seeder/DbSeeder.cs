using app.webapi.backoffice_viajes_altairis.Domain.Enums;
using app.webapi.backoffice_viajes_altairis.Domain.Models;
using app.webapi.backoffice_viajes_altairis.Domain.ValueObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace app.webapi.backoffice_viajes_altairis.Data.Seeder
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AltarisDbContext context)
        {
            Console.WriteLine("--> ENTRANDO AL SEEDER...");

            if (await context.Hotels.AnyAsync())
            {
                Console.WriteLine("--> EL SEEDER DETECTÓ DATOS Y SE SALIÓ.");
                return;
            }

            int numberRegister = 100;
            var random = new Random();
            var hotels = new List<Hotel>();
            var allReservations = new List<Reservation>();

            // --- Listas de ayuda para datos aleatorios ---
            var cities = new[] {
                new { City= "Madrid", Country = "España" },
                new { City= "Barcelona", Country = "España" },
                new { City= "Valencia", Country = "España" },
                new { City= "Sevilla", Country = "España" },
                new { City= "Bilbao", Country = "España" },
                new { City= "Malaga", Country = "España" },
                new { City= "Granada", Country = "España" },
                new { City= "San Sebastian", Country = "España" },
                new { City= "Salamanca", Country = "España" },
                new { City= "Toledo", Country = "España" }
            };

            var hotelPrefixes = new[] { "Grand", "Royal", "Palace", "Plaza", "Boutique", "Urban", "Garden", "Seaside", "Mountain", "Historic" };
            var hotelSuffixes = new[] { "Hotel", "Resort", "Inn", "Suites", "Residences" };
            var firstNames = new[] { "Carlos", "Maria", "Juan", "Ana", "Pedro", "Laura", "Miguel", "Elena", "Diego", "Carmen" };
            var lastNames = new[] { "Garcia", "Martinez", "Lopez", "Sanchez", "Gonzalez", "Rodriguez", "Fernandez", "Perez" };

            var allServices = new[] {
                "WiFi", "Baño privado", "TV", "Minibar", "Aire acondicionado",
                "Caja fuerte", "Secador de pelo", "Vista ciudad", "Vista mar",
                "Terraza", "Sala estar", "Jacuzzi", "Balcón", "Cocina"
            };

            var roomTemplates = new[] {
                new { Type = TypeRooms.Individual, BasePrice = 60m },
                new { Type = TypeRooms.Double,     BasePrice = 90m },
                new { Type = TypeRooms.Deluxe,     BasePrice = 120m },
                new { Type = TypeRooms.Suite,      BasePrice = 180m },
                new { Type = TypeRooms.Ejecutiva,  BasePrice = 250m }
            };

            // --- Generación de Hoteles ---
            for (int i = 1; i <= numberRegister; i++)
            {
                var location = cities[random.Next(cities.Length)];
                var hotel = new Hotel
                {
                    Id = Guid.NewGuid(),
                    Name = $"{hotelPrefixes[random.Next(hotelPrefixes.Length)]} {location.City} {hotelSuffixes[random.Next(hotelSuffixes.Length)]} #{i}",
                    Address = $"Calle Principal {random.Next(1, 200)}",
                    City = location.City,
                    Country = location.Country,
                    Stars = random.Next(3, 6),
                    PhoneNumber = $"+34 {random.Next(600000000, 700000000)}",
                    Email = $"info@hotel{i}.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 365))
                };

                // --- Generación de Habitaciones ---
                var numRooms = random.Next(2, 5);
                for (int j = 0; j < numRooms; j++)
                {
                    var template = roomTemplates[random.Next(roomTemplates.Length)];
                    var assignedServices = allServices.OrderBy(x => random.Next()).Take(random.Next(3, 9)).ToList();

                    var room = new Room
                    {
                        Id = Guid.NewGuid(),
                        HotelId = hotel.Id,
                        Name = template.Type.Code,
                        Description = template.Type.Name,
                        Capacity = template.Type.Capacity,
                        Price = template.BasePrice * (hotel.Stars == 5 ? 1.5m : 1.0m),
                        Quantity = random.Next(1, 6),
                        IsActive = true,
                        Services = assignedServices,
                        CreatedAt = DateTime.UtcNow
                    };
                    hotel.Rooms.Add(room);
                }

                hotel.TotalRooms = hotel.Rooms.Sum(r => r.Quantity);
                hotels.Add(hotel);

                // --- Generación de Reservas ---
                var numReservations = random.Next(40, 91);
                for (int k = 0; k < numReservations; k++)
                {
                    var randomRoom = hotel.Rooms[random.Next(hotel.Rooms.Count)];
                    var checkIn = DateTime.UtcNow.AddDays(random.Next(-20, 41));
                    int nights = random.Next(3, 8);
                    var checkOut = checkIn.AddDays(nights);

                    TypeStatusReservation assignedStatus;
                    if (checkOut < DateTime.UtcNow)
                    {
                        assignedStatus = random.Next(1, 11) <= 8 ? TypeStatusReservation.COMPLETED : TypeStatusReservation.CANCELLED;
                    }
                    else if (checkIn <= DateTime.UtcNow && checkOut >= DateTime.UtcNow)
                    {
                        assignedStatus = TypeStatusReservation.ACTIVE;
                    }
                    else
                    {
                        assignedStatus = TypeStatusReservation.PENDING;
                    }

                    allReservations.Add(new Reservation
                    {
                        Id = Guid.NewGuid(),
                        RoomId = randomRoom.Id,
                        GuestFullName = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}",
                        GuestEmail = "test@example.com",
                        PhoneNumber = $"+34 {random.Next(600000000, 700000000)}",
                        CheckIn = checkIn,
                        CheckOut = checkOut,
                        Price = randomRoom.Price * nights,
                        StatusReservation = assignedStatus,
                        CreatedAt = checkIn.AddDays(-random.Next(1, 10))
                    });
                }
            }

            // --- MOTOR DE OCUPACIÓN AJUSTADO PARA ALTA DISPONIBILIDAD ---
            Console.WriteLine("--> CALCULANDO OCUPACIÓN POR DÍA...");
            
            // Creamos un diccionario de habitaciones para acceso rápido y coherencia de stock
            var roomLookup = hotels.SelectMany(h => h.Rooms).ToDictionary(r => r.Id);

            var occupancyData = allReservations
                .Where(r => r.StatusReservation != TypeStatusReservation.CANCELLED)
                .SelectMany(r => Enumerable.Range(0, (r.CheckOut - r.CheckIn).Days)
                    .Select(offset => new { r.RoomId, Date = r.CheckIn.AddDays(offset).Date }))
                .GroupBy(x => new { x.RoomId, x.Date })
                .Select(g => new RoomOccupancy
                {
                    Id = Guid.NewGuid(),
                    RoomId = g.Key.RoomId,
                    Date = g.Key.Date,
                    // Alta disponibilidad: permitimos que se vea la ocupación real según tus reservas
                    OccupiedCount = g.Count() 
                }).ToList();

            // Inserción masiva para optimizar rendimiento
            await context.Hotels.AddRangeAsync(hotels);
            await context.Reservations.AddRangeAsync(allReservations);
            await context.RoomOccupancies.AddRangeAsync(occupancyData);
            await context.SaveChangesAsync();
        }
    }
}