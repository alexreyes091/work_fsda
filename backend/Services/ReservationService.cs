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
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomOccupancyService _occupancyService;
        private readonly ReservationValidator _reservationValidator;
        private readonly IMapper _mapper;

        public ReservationService(
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository, 
            IRoomOccupancyService occupancyService ,
            ReservationValidator reservationValidator, 
            IMapper mapper
        ){
            _reservationRepository = reservationRepository;
            _reservationValidator = reservationValidator;
            _occupancyService = occupancyService;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        public async Task<Result<ReservationDto>> CreateReservation(CreateReservationDto dataReservation)
        {
            Reservation reservationData = _mapper.Map<Reservation>(dataReservation);
            const int INCREMENT_VALUE = 1;

            //TODO: Completar todas las validaciones de la reservación dentro del fluentValidation
            var validationResult = _reservationValidator.Validate(reservationData);
            if(!validationResult.IsValid)
            {
                string errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<ReservationDto>.Failure($"Validación fallida: {errors}", "ValidationError");
            }

            // Se valida la disponibilidad de la habitación
            bool isRoomAvailable = await _occupancyService.IsAvailableAsync(
                reservationData.RoomId, 
                reservationData.CheckIn, 
                reservationData.CheckOut
            );

            if( !isRoomAvailable )
                return Result<ReservationDto>.Failure(
                    "Lo sentimos, no hay cupo disponible para este tipo de habitación en las fechas seleccionadas.", 
                    TypeResultResponse.VALIDATION_ERROR.ToString());

            // Actualizar/Creamos disponibilidad/reserva de la habitación
            bool isSaved = await _reservationRepository.CreateAsync(reservationData);

            if (isSaved)
            {
                await _occupancyService.UpdateInventoryAsync(
                    reservationData.RoomId,
                    reservationData.CheckIn,
                    reservationData.CheckOut, 
                    INCREMENT_VALUE);

                return Result<ReservationDto>.Success(_mapper.Map<ReservationDto>(reservationData));
            }

            return Result<ReservationDto>.Failure(
                "Error al crear la reservación. Por favor, inténtelo de nuevo más tarde.",
                TypeResultResponse.ERROR_EXCEPTION.ToString());
        }

        public async Task<PagedResult<ReservationDto>> GetAllReservation(int numberPage, int pageSize)
        {
            var (reservation, totalRecord) = await _reservationRepository.GetPagedAsync(numberPage, pageSize);
            return PagedResult<ReservationDto>.Success(reservation, totalRecord, numberPage, pageSize);

        }

        public async Task<PagedResult<ReservationDto>> GetAllReservationByHotel(Guid hotelId, int numberPage, int pageSize)
        {
            var (reservations, totalRecord) = await _reservationRepository.GetPagedByHotelAsync(hotelId, numberPage, pageSize);
            return PagedResult<ReservationDto>.Success(reservations, totalRecord, numberPage, pageSize);
        }

        public async Task<PagedResult<ReservationDto>> GetReservationByRangeDate(string startDate, string endDate, int numberPage, int pageSize)
        {
            if (!DateTime.TryParse(startDate, out DateTime start) || !DateTime.TryParse(endDate, out DateTime end))
                return PagedResult<ReservationDto>.Success([], 0, numberPage, pageSize);

            var (reservations, totalRecord) = await _reservationRepository.GetPagedByDateRangeAsync(start, end, numberPage, pageSize);

            return PagedResult<ReservationDto>.Success(reservations, totalRecord, numberPage, pageSize);
        }
    }
}
