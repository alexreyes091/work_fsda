using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using app.webapi.backoffice_viajes_altairis.Domain.Models;

namespace app.webapi.backoffice_viajes_altairis.Data.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        /// <summary>
        /// Asynchronously retrieves a paged collection of rooms and the total number of available records.
        /// </summary>
        /// <param name="pageNumber">The one-based index of the page to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The maximum number of rooms to include in the returned page. Must be greater than 0.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with an enumerable
        /// collection of rooms for the specified page and the total number of available records.</returns>
        Task<(IEnumerable<RoomDto> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Asynchronously retrieves all rooms associated with the specified hotel identifier.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel for which to retrieve rooms.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of rooms belonging
        /// to the specified hotel. The collection is empty if no rooms are found.</returns>
        Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId);
    }
}
