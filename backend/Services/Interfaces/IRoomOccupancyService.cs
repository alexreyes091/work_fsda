using app.webapi.backoffice_viajes_altairis.Common;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using Sprache;

namespace app.webapi.backoffice_viajes_altairis.Services.Interfaces
{

    public interface IRoomOccupancyService
    {
        /// <summary>
        /// Asynchronously retrieves the occupancy grid for a specified room within a given date range.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the room for which occupancy data is requested.</param>
        /// <param name="startDate">The start date of the range for which to retrieve occupancy information. Must be less than or equal to
        /// <paramref name="endDate"/>.</param>
        /// <param name="endDate">The end date of the range for which to retrieve occupancy information. Must be greater than or equal to
        /// <paramref name="startDate"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/>
        /// wrapping a collection of <see cref="RoomOccupancyDto"/> objects representing occupancy data for each day in
        /// the specified range.</returns>
        Task<Result<IEnumerable<RoomOccupancyDto>>> GetOccupancyGridAsync(Guid hotelId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Asynchronously determines whether the specified room is available for the given date range.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room to check for availability.</param>
        /// <param name="checkIn">The start date and time of the desired booking period.</param>
        /// <param name="checkOut">The end date and time of the desired booking period. Must be later than <paramref name="checkIn"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the room is
        /// available for the specified period; otherwise, <see langword="false"/>.</returns>
        Task<bool> IsAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut);

        /// <summary>
        /// Asynchronously updates the inventory count for a specified room and date range.
        /// </summary>
        /// <remarks>If the specified date range overlaps with existing reservations, the update may fail.
        /// This method is thread-safe and can be called concurrently for different rooms.</remarks>
        /// <param name="roomId">The unique identifier of the room whose inventory will be updated.</param>
        /// <param name="checkIn">The start date of the inventory update period, inclusive.</param>
        /// <param name="checkOut">The end date of the inventory update period, exclusive. Must be later than <paramref name="checkIn"/>.</param>
        /// <param name="increment">The number by which to adjust the inventory. Positive values increase inventory; negative values decrease
        /// it.</param>
        /// <returns>A task that represents the asynchronous operation. The result contains the outcome of the inventory update,
        /// including success or failure details.</returns>
        Task<bool> UpdateInventoryAsync(Guid roomId, DateTime checkIn, DateTime checkOut, int increment);

        /// <summary>
        /// Initializes the inventory for the specified room starting from the given date with the provided initial
        /// stock value.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room for which the inventory will be initialized.</param>
        /// <param name="startDate">The date from which the inventory tracking should begin.</param>
        /// <param name="initialStock">The initial quantity of stock to set for the room's inventory. Must be zero or greater.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the
        /// inventory was successfully initialized; otherwise, <see langword="false"/>.</returns>
        Task<bool> InitializeInventoryAsync(Guid roomId, DateTime startDate, int initialStock);
    }
}
