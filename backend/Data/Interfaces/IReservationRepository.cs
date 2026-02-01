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
        Task<(IEnumerable<Reservation> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize, Guid? hotelId = null, DateTime? date = null);
    }
}
