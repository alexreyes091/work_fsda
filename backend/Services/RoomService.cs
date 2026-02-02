using app.webapi.backoffice_viajes_altairis.Common;
using app.webapi.backoffice_viajes_altairis.Common.Enum;
using app.webapi.backoffice_viajes_altairis.Data.Interfaces;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using app.webapi.backoffice_viajes_altairis.Domain.Models;
using app.webapi.backoffice_viajes_altairis.Domain.Validators;
using app.webapi.backoffice_viajes_altairis.Services.Interfaces;
using MapsterMapper;

namespace app.webapi.backoffice_viajes_altairis.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly RoomValidator _roomValidator;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, RoomValidator roomValidator,  IMapper mapper, IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _roomValidator = roomValidator;
            _mapper = mapper;
        }

        public async Task<Result<RoomDto>> CreateRoom(CreateRoomDto room)
        {
            Room roomData = _mapper.Map<Room>(room);
            var validationResult = _roomValidator.Validate(roomData);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<RoomDto>.Failure(validationResult.ToString(), TypeResultResponse.VALIDATION_ERROR.ToString());
            }

            if(!await _hotelRepository.ExistAsync(roomData.Hotel.Name))
                return Result<RoomDto>.Failure(
                    "Hotel no existe, no puede crear la habitación",
                    TypeResultResponse.VALIDATION_ERROR.ToString());
           
            if(await _roomRepository.ExistAsync(roomData.Name))
                return Result<RoomDto>.Failure(
                    "Ya existe una habitación con ese nombre",
                    TypeResultResponse.VALIDATION_ERROR.ToString());

            bool isSaved = await _roomRepository.CreateAsync(roomData);

            return isSaved
                ? Result<RoomDto>.Success(_mapper.Map<RoomDto>(roomData))
                : Result<RoomDto>.Failure("Error al crear la habitación",TypeResultResponse.ERROR_EXCEPTION.ToString());
        }

        public async Task<PagedResult<RoomDto>> GetAllRooms(int numberPage, int pageSize)
        {
            var (rooms, totalCount) =  await _roomRepository.GetPagedAsync(numberPage, pageSize);

            return PagedResult<RoomDto>.Success(rooms, totalCount, numberPage, pageSize);

        }

        public async Task<Result<IEnumerable<RoomDto>>> GetAllRoomsByHotelName(string name)
        {
            Hotel? dataHotel = await _hotelRepository.GetByNameAsync(name);
            
            if (dataHotel == null)
                return Result<IEnumerable<RoomDto>>.Failure(
                    "El hotel ingresado no existe", 
                    TypeResultResponse.NOT_FOUND.ToString());

            IEnumerable<RoomDto> dataRooms = 
                _mapper.Map<IEnumerable<RoomDto>>(await _roomRepository.GetByHotelIdAsync(dataHotel.Id));

            return Result<IEnumerable<RoomDto>>.Success(dataRooms);
            
        }
    }
}
