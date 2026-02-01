using app.webapi.backoffice_viajes_altairis.Domain.Models;

namespace app.webapi.backoffice_viajes_altairis.Data.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        /// <summary>
        /// Asynchronously retrieves a paged collection of hotels and the total number of available records.
        /// </summary>
        /// <param name="pageNumber">The one-based index of the page to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The maximum number of hotel records to include in the page. Must be greater than 0.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with an enumerable
        /// collection of hotels for the specified page and the total number of hotel records available.</returns>
        Task<(IEnumerable<Hotel> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize);
    }
}
