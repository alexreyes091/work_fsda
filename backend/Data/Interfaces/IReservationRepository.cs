using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using app.webapi.backoffice_viajes_altairis.Domain.Models;

namespace app.webapi.backoffice_viajes_altairis.Data.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        /// <summary>
        /// Retrieves a paged list of reservations, optionally filtered by hotel and reservation date.
        /// </summary>
        /// <param name="pageNumber">The one-based page number to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The maximum number of reservations to include in the page. Must be greater than 0.</param>
        /// <param name="hotelId">The unique identifier of the hotel to filter reservations by. If null, reservations from all hotels are
        /// included.</param>
        /// <param name="date">The reservation date to filter by. If null, reservations from all dates are included.</param>
        /// <returns>A task that represents the asynchronous operation. The result contains a tuple with an enumerable collection
        /// of reservations for the specified page and the total number of matching reservations.</returns>
        Task<(IEnumerable<ReservationDto> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize, Guid? hotelId = null, DateTime? date = null);
        /// <summary>
        /// Asynchronously retrieves a paged list of reservations for the specified hotel.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel for which to retrieve reservations.</param>
        /// <param name="numberPage">The zero-based page number to retrieve. Must be greater than or equal to 0.</param>
        /// <param name="pageSize">The number of reservations to include in each page. Must be greater than 0.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with an enumerable
        /// collection of reservations for the specified page and the total number of reservation records for the hotel.</returns>
        Task<(IEnumerable<Reservation> reservations, int totalRecord)> GetPagedByHotelAsync(Guid hotelId, int numberPage, int pageSize);
        /// <summary>
        /// Asynchronously retrieves a paged collection of reservations that fall within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range. Only reservations on or after this date are included.</param>
        /// <param name="endDate">The end date of the range. Only reservations on or before this date are included.</param>
        /// <param name="numberPage">The zero-based index of the page to retrieve. Must be greater than or equal to 0.</param>
        /// <param name="pageSize">The maximum number of reservations to include in a single page. Must be greater than 0.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with a collection of
        /// reservations for the specified page and the total number of records matching the date range.</returns>
        Task<(IEnumerable<Reservation> reservations, int totalRecord)> GetPagedByDateRangeAsync(DateTime startDate, DateTime endDate, int numberPage, int pageSize);
    }
}
