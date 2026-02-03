using app.webapi.backoffice_viajes_altairis.Data.Interfaces;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using app.webapi.backoffice_viajes_altairis.Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace app.webapi.backoffice_viajes_altairis.Data.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AltarisDbContext _contextAltaris;

        public RoomRepository(AltarisDbContext contextAltaris)
            => _contextAltaris = contextAltaris;
        
        public async Task<bool> CreateAsync(Room entity)
        {
            await _contextAltaris.Rooms.AddAsync(entity);
            return await SaveAsync();
        }

        public Task<bool> DeleteAsync(Room entity)
        {
            entity.IsActive = false;
            _contextAltaris.Rooms.Update(entity);
            return SaveAsync();
        }

        public async Task<bool> ExistAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            string searchName = name.Trim().ToUpper();
            return await _contextAltaris.Rooms.AsNoTracking()
                .AnyAsync(x => x.Name.ToUpper() == searchName);
        }

        public async Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId)
        {
            return await _contextAltaris.Rooms
                .Where(x => x.HotelId == hotelId && x.IsActive)
                .OrderBy(x => x.Name)
                .AsNoTracking().ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(Guid id)
        {
            return await _contextAltaris.Rooms.AsNoTracking()
                .Include(x => x.Hotel)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Room?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            string searchName = name.Trim().ToUpper();
            return await _contextAltaris.Rooms.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == searchName);
        }

        public async Task<(IEnumerable<RoomDto> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _contextAltaris.Rooms
                .Where(x => x.IsActive)
                .AsNoTracking();

            int totalRecords = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<RoomDto>()
                .ToListAsync();

            return (data, totalRecords);
        }

        public IQueryable<Room> Query()
            => _contextAltaris.Rooms.AsNoTracking();

        public async Task<bool> SaveAsync()
            => (await _contextAltaris.SaveChangesAsync() > 0);

        public async Task<bool> UpdateAsync(Room entity)
        {
            _contextAltaris.Rooms.Update(entity);
            return await SaveAsync();
        }
    }
}
