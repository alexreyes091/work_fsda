using app.webapi.backoffice_viajes_altairis.Domain.Models;

namespace app.webapi.backoffice_viajes_altairis.Data.Interfaces
{
    public interface IRoomOccupancyRepository : IRepository<RoomOccupancy>
    {
        /// <summary>
        /// Asynchronously retrieves occupancy records for the specified room within the given date and time range.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room for which occupancy data is requested.</param>
        /// <param name="start">The start of the date and time range to filter occupancy records. Must be less than or equal to <paramref
        /// name="end"/>.</param>
        /// <param name="end">The end of the date and time range to filter occupancy records. Must be greater than or equal to <paramref
        /// name="start"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
        /// cref="RoomOccupancy"/> instances for the specified room and time range. The collection will be empty if no
        /// records are found.</returns>
        public Task<IEnumerable<RoomOccupancy>> GetByRangeAsync(IEnumerable<Guid> roomId, DateTime start, DateTime end);

        /// <summary>
        /// Asynchronously determines whether the specified room is available for the given date range and can
        /// accommodate the requested maximum capacity.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room to check for availability.</param>
        /// <param name="checkIn">The start date and time of the desired booking period.</param>
        /// <param name="checkOut">The end date and time of the desired booking period. Must be later than <paramref name="checkIn"/>.</param>
        /// <param name="maxCapacity">The maximum number of guests the room must be able to accommodate. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the room is
        /// available for the specified period and capacity; otherwise, <see langword="false"/>.</returns>
        public Task<bool> CheckAvailabilityAsync(Guid roomId, DateTime checkIn, DateTime checkOut, int maxCapacity);

        /// <summary>
        /// Updates the occupancy count for a specified room over a given date range asynchronously.
        /// </summary>
        /// <remarks>If the specified date range overlaps with existing occupancy records, the occupancy
        /// count will be adjusted accordingly. The method does not validate whether the resulting occupancy exceeds
        /// room capacity; callers should ensure that business rules are enforced as needed.</remarks>
        /// <param name="roomId">The unique identifier of the room whose occupancy is to be updated.</param>
        /// <param name="checkIn">The start date and time of the occupancy period. Represents the check-in date, inclusive.</param>
        /// <param name="checkOut">The end date and time of the occupancy period. Represents the check-out date, exclusive.</param>
        /// <param name="increment">The value by which to adjust the occupancy count. Can be positive or negative to increase or decrease
        /// occupancy.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task UpdateOccupancyAsync(Guid roomId, DateTime checkIn, DateTime checkOut, int increment);
    }
}
