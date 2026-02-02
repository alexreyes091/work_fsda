using app.webapi.backoffice_viajes_altairis.Common;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;

namespace app.webapi.backoffice_viajes_altairis.Services.Interfaces
{
    public interface IRoomService
    {
        /// <summary>
        /// Creates a new room using the specified data transfer object.
        /// </summary>
        /// <param name="entity">An object containing the details required to create the room. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{RoomDto}"/>
        /// indicating the outcome of the operation and, if successful, the details of the created room.</returns>
        public Task<Result<RoomDto>> CreateRoom(CreateRoomDto entity);
        /// <summary>
        /// Retrieves a paginated list of rooms for the specified page number and page size.
        /// </summary>
        /// <param name="numberPage">The zero-based index of the page to retrieve. Must be greater than or equal to 0.</param>
        /// <param name="pageSize">The maximum number of rooms to include in the returned page. Must be greater than 0.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="PagedResult{RoomDto}"/> with the rooms for the requested page. If no rooms exist for the specified
        /// page, the result will contain an empty collection.</returns>
        public Task<PagedResult<RoomDto>> GetAllRooms(int numberPage, int pageSize);

        /// <summary>
        /// Asynchronously retrieves all rooms associated with the specified hotel.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel for which to retrieve rooms.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{RoomDto}"/>
        /// with the collection of rooms for the specified hotel. If no rooms are found, the collection will be empty.</returns>
        public Task<Result<IEnumerable<RoomDto>>> GetAllRoomsByHotelName(string name);

    }
}
