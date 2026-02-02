using app.webapi.backoffice_viajes_altairis.Data.Interfaces;
using app.webapi.backoffice_viajes_altairis.Domain.Enums;
using app.webapi.backoffice_viajes_altairis.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace app.webapi.backoffice_viajes_altairis.Data.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AltarisDbContext _contextAltaris;
        
        public ReservationRepository(AltarisDbContext contextAltaris)
            => _contextAltaris = contextAltaris;

        public async Task<bool> CreateAsync(Reservation entity)
        {
            await _contextAltaris.Reservations.AddAsync(entity);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Reservation entity)
        {
            entity.StatusReservation = TypeStatusReservation.CANCELLED;
            entity.CancelledAt = DateTime.UtcNow;

            _contextAltaris.Reservations.Update(entity);
            return await SaveAsync();
        }

        public async Task<Reservation?> GetByIdAsync(Guid id)
            => await _contextAltaris.Reservations.AsNoTracking()
                .Include(x => x.Room)
                .ThenInclude(r => r.Hotel)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<(IEnumerable<Reservation> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize, Guid? hotelId = null, DateTime? date = null)
        {
            var query = _contextAltaris.Reservations
                .Include(x => x.Room)
                .ThenInclude(r => r.Hotel)
                .AsNoTracking();

            if(hotelId.HasValue)
                query = query.Where(r => r.Room.HotelId == hotelId.Value);

            if(date.HasValue)
                query = query.Where(r => r.CheckIn <= date.Value && r.CheckOut >= date.Value);

            int totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }

        public IQueryable<Reservation> Query()
        {
            return _contextAltaris.Reservations.AsNoTracking()
                .Include(x => x.Room)
                .ThenInclude(r => r.Hotel);
        }

        public async Task<bool> SaveAsync()
            => (await _contextAltaris.SaveChangesAsync() > 0);
        

        public async Task<bool> UpdateAsync(Reservation entity)
        {
            _contextAltaris.Reservations.Update(entity);
            return await SaveAsync();
        }
        public Task<bool> ExistAsync(string name) => Task.FromResult(false);
        public Task<Reservation?> GetByNameAsync(string name) => Task.FromResult<Reservation?>(null);

        public async Task<(IEnumerable<Reservation> reservations, int totalRecord)> GetPagedByHotelAsync(Guid hotelId, int numberPage, int pageSize)
        {
            var query = _contextAltaris.Reservations
                .Include(r => r.Room)
                .Where(r => r.Room.HotelId == hotelId)
                .AsNoTracking();

            int totalRecord = await query.CountAsync();

            var reservations = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((numberPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (reservations, totalRecord);
        }

        public async Task<(IEnumerable<Reservation> reservations, int totalRecord)> GetPagedByDateRangeAsync(DateTime startDate, DateTime endDate, int numberPage, int pageSize)
        {
            var query = _contextAltaris.Reservations
                .Where(r => r.CheckIn < endDate && r.CheckOut > startDate)
                .AsNoTracking();

            int totalRecord = await query.CountAsync();

            var reservations = await query
                .OrderBy(r => r.CheckIn)
                .Skip((numberPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (reservations, totalRecord);
        }
    }
}
