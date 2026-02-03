using app.webapi.backoffice_viajes_altairis.Data;
using app.webapi.backoffice_viajes_altairis.Data.Interfaces;
using app.webapi.backoffice_viajes_altairis.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class RoomOccupancyRepository : IRoomOccupancyRepository
{
    private readonly AltarisDbContext _context;

    public RoomOccupancyRepository(AltarisDbContext altarisContext)
    {
        _context = altarisContext;
    }

    public async Task<bool> CheckAvailabilityAsync(Guid roomId, DateTime checkIn, DateTime checkOut, int maxCapacity)
    {
        return !await _context.RoomOccupancies
            .AnyAsync(x => x.RoomId == roomId
                        && x.Date >= checkIn
                        && x.Date < checkOut
                        && x.OccupiedCount >= maxCapacity);
    }

    public async Task<IEnumerable<RoomOccupancy>> GetByRangeAsync(IEnumerable<Guid> roomIds, DateTime startDate, DateTime endDate)
    {
        return await _context.RoomOccupancies
            .Where(x => 
                roomIds.Contains(x.RoomId) && 
                x.Date >= startDate && 
                x.Date <= endDate)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateOccupancyAsync(Guid roomId, DateTime checkIn, DateTime checkOut, int increment)
    {
        // La idea es que se pueda iterar por cada noche (CheckOut no se cuenta como noche ocupada)
        for (var date = checkIn.Date; date < checkOut.Date; date = date.AddDays(1))
        {
            var occupancy = await _context.RoomOccupancies
                .FirstOrDefaultAsync(x => x.RoomId == roomId && x.Date == date);

            if (occupancy == null)
            {
                await _context.RoomOccupancies.AddAsync(new RoomOccupancy
                {
                    Id = Guid.NewGuid(),
                    RoomId = roomId,
                    Date = date,
                    OccupiedCount = increment
                });
            }
            else
            {
                occupancy.OccupiedCount += increment;
            }
        }
    }
    public async Task<bool> CreateAsync(RoomOccupancy entity)
    {
        // TODO: Verificar si ya existe una ocupación para la misma habitación y fecha
        // TODO: Agregar mas validaciones 
        await _context.RoomOccupancies.AddAsync(entity);
        return await SaveAsync();
    }

    public async Task<bool> UpdateAsync(RoomOccupancy entity)
    {
        // TODO: Agregar mas validaciones 
        _context.RoomOccupancies.Update(entity);
        return await SaveAsync();
    }

    public async Task<bool> DeleteAsync(RoomOccupancy entity)
    {
        _context.RoomOccupancies.Remove(entity);
        return await SaveAsync();
    }

    public async Task<RoomOccupancy?> GetByIdAsync(Guid id)
        => await _context.RoomOccupancies.FindAsync(id);

    public IQueryable<RoomOccupancy> Query()
        => _context.RoomOccupancies.AsNoTracking();

    public async Task<bool> SaveAsync()
        => await _context.SaveChangesAsync() > 0;

    // Métodos no aplican para este repositorio
    public Task<bool> ExistAsync(string name) => throw new NotSupportedException("Búsqueda por nombre no aplica a ocupación");
    public Task<RoomOccupancy?> GetByNameAsync(string name) => throw new NotSupportedException();
}