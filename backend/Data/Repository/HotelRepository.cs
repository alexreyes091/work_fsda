using app.webapi.backoffice_viajes_altairis.Data.Interfaces;
using app.webapi.backoffice_viajes_altairis.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace app.webapi.backoffice_viajes_altairis.Data.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AltarisDbContext _contextAltaris;
        public HotelRepository(AltarisDbContext contextAltaris) => _contextAltaris = contextAltaris;
        
        public async Task<bool> CreateAsync(Hotel entity)
        {
            await _contextAltaris.Hotels.AddAsync(entity);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Hotel entity)
        {
            entity.IsActive = false;
            return await UpdateAsync(entity);
        }

        public async Task<bool> ExistAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            string searchName = name.Trim().ToUpper();

            return await _contextAltaris.Hotels
                    .AsNoTracking()
                    .AnyAsync(x => x.Name.ToUpper() == searchName);
        }

        public async Task<Hotel?> GetByIdAsync(Guid id)
        {
            return await _contextAltaris.Hotels.AsNoTracking()
                .Include(r => r.Rooms)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Hotel?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            string searchName = name.Trim().ToUpper();
            return await _contextAltaris.Hotels.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == searchName);
        }

        public async Task<(IEnumerable<Hotel> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _contextAltaris.Hotels
                .Where(x => x.IsActive).AsNoTracking();

            int totalRecords = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return (data, totalRecords);
        }

        public IQueryable<Hotel> Query() 
            => _contextAltaris.Hotels.AsNoTracking();

        public async Task<bool> SaveAsync() 
            => (await _contextAltaris.SaveChangesAsync() > 0);

        public async Task<bool> UpdateAsync(Hotel entity)
        {
            _contextAltaris.Hotels.Update(entity);
            return await SaveAsync();
        }
    }
}