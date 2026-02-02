using app.webapi.backoffice_viajes_altairis.Common;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using app.webapi.backoffice_viajes_altairis.Domain.Models;

namespace app.webapi.backoffice_viajes_altairis.Services.Interfaces
{
    public interface IReservationService
    {
        /// <summary>
        /// Retrieves a paged list of reservations based on the specified page number and page size.
        /// </summary>
        /// <param name="numberPage">The zero-based index of the page to retrieve. Must be greater than or equal to 0.</param>
        /// <param name="pageSize">The number of reservations to include in each page. Must be greater than 0.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="PagedResult{ReservationDto}"/> with the reservations for the specified page. If no reservations exist
        /// for the page, the result contains an empty collection.</returns>
        public Task<PagedResult<ReservationDto>> GetAllReservation(int numberPage, int pageSize);
        /// <summary>
        /// Retrieves a paged list of reservations associated with the specified hotel.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel for which to retrieve reservations. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paged result of reservation
        /// data transfer objects for the specified hotel. If no reservations are found, the result contains an empty
        /// collection.</returns>
        public Task<PagedResult<ReservationDto>> GetAllReservationByHotel(Guid hotelId, int numberPage, int pageSize);
        /// <summary>
        /// Retrieves a paged list of reservations that fall within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range, in ISO 8601 format (yyyy-MM-dd). Reservations with a date on or after this
        /// value are included.</param>
        /// <param name="endDate">The end date of the range, in ISO 8601 format (yyyy-MM-dd). Reservations with a date on or before this value
        /// are included.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paged result of reservations
        /// matching the specified date range. If no reservations are found, the result contains an empty collection.</returns>
        public Task<PagedResult<ReservationDto>> GetReservationByRangeDate(string startDate, string endDate, int numberPage, int pageSize);
        /// <summary>
        /// Creates a new reservation using the specified reservation details.
        /// </summary>
        /// <param name="dataReservation">An object containing the details required to create the reservation. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a Result object with the created
        /// reservation details if successful; otherwise, contains error information.</returns>
        public Task<Result<ReservationDto>> CreateReservation(CreateReservationDto dataReservation);
    }
}
