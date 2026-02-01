using app.webapi.backoffice_viajes_altairis.Common;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;

namespace app.webapi.backoffice_viajes_altairis.Services.Interfaces
{
    public interface IHotelService
    {
        public Task<Result<HotelDto>> GetHotelByName(string name);
        public Task<PagedResult<HotelDto>> GetAllHotels(int numberPage, int pageSize);

        public Task<Result<HotelDto>> CreateHotel(CreateHotelDto dataHotel);

    }
}
