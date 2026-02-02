using app.webapi.backoffice_viajes_altairis.Common;
using app.webapi.backoffice_viajes_altairis.Common.Enum;
using app.webapi.backoffice_viajes_altairis.Data;
using app.webapi.backoffice_viajes_altairis.Data.Interfaces;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using app.webapi.backoffice_viajes_altairis.Domain.Models;
using app.webapi.backoffice_viajes_altairis.Services.Interfaces;
using MapsterMapper;

namespace app.webapi.backoffice_viajes_altairis.Services
{
    public class RoomOccupancyService : IRoomOccupancyService
    {
        private readonly IRoomOccupancyRepository _occupancyRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomOccupancyService(
            IRoomOccupancyRepository occupancyRepository,
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _occupancyRepository = occupancyRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<RoomOccupancyDto>>> GetOccupancyGridAsync(Guid roomId, DateTime startDate, DateTime endDate)
        {
            Room? dataRoom = await _roomRepository.GetByIdAsync(roomId);
            if (dataRoom == null)
                return Result<IEnumerable<RoomOccupancyDto>>.Failure("Habitación no encontrada.", TypeResultResponse.NOT_FOUND.ToString());


            IEnumerable<RoomOccupancy> occupancies = await _occupancyRepository.GetByRangeAsync(roomId, startDate, endDate);

            // Grid para crear un estilo mapa de calor
            var dataGrid = new List<RoomOccupancyDto>();
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var existing = occupancies.FirstOrDefault(x => x.Date.Date == date);

                dataGrid.Add(new RoomOccupancyDto
                {
                    Date = date,
                    OccupiedCount = existing?.OccupiedCount ?? 0,
                    TotalRooms = dataRoom.Quantity,
                    RoomName = dataRoom.Name
                });
            }

            return Result<IEnumerable<RoomOccupancyDto>>.Success(dataGrid);
        }

        public async Task<bool> IsAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut)
        {
            Room? dataRoom = await _roomRepository.GetByIdAsync(roomId);
            if (dataRoom == null) return false;

            return await _occupancyRepository.CheckAvailabilityAsync(roomId, checkIn, checkOut, dataRoom.Quantity);
        }

        public async Task<bool> UpdateInventoryAsync(Guid roomId, DateTime checkIn, DateTime checkOut, int increment)
        {
            // TODO: Agregar validaciones necesarias.
            await _occupancyRepository.UpdateOccupancyAsync(roomId, checkIn, checkOut, increment);
            return await _occupancyRepository.SaveAsync();
        }

        public async Task<bool> InitializeInventoryAsync(Guid roomId, DateTime startDate, int initialStock)
        {
            RoomOccupancy occupancy = new()
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                Date = startDate.Date,
                OccupiedCount = initialStock
            };

            return await _occupancyRepository.CreateAsync(occupancy);
        }
    }
}
