using app.webapi.backoffice_viajes_altairis.Common;
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
        private readonly ReservationValidator _reservationValidator;
        private readonly IMapper _mapper;

        public ReservationService(IReservationRepository reservationRepository,IRoomRepository roomRepository, ReservationValidator reservationValidator, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _reservationValidator = reservationValidator;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        public async Task<Result<ReservationDto>> CreateReservation(CreateReservationDto dataReservation)
        {
            Reservation reservationData = _mapper.Map<Reservation>(dataReservation);

            //TODO: Completar todas las validaciones de la reservación
            var validationResult = _reservationValidator.Validate(reservationData);
            if(!validationResult.IsValid)
            {
                string errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<ReservationDto>.Failure($"Validación fallida: {errors}", "ValidationError");
            }

            // TODO: Verificar disponibilidad de la habitación para las fechas dadas
            // TODO: Actualizar disponibilidad de la habitación

            bool isSaved = await _reservationRepository.CreateAsync(reservationData);

            return isSaved
                ? Result<ReservationDto>.Success(_mapper.Map<ReservationDto>(reservationData))
                : Result<ReservationDto>.Failure("No se pudo crear la reservación", "CreationError");
        }

        public async Task<PagedResult<ReservationDto>> GetAllReservation(int numberPage, int pageSize)
        {
            var (reservation, totalRecord) = await _reservationRepository.GetPagedAsync(numberPage, pageSize);
            IEnumerable<ReservationDto> reservationDto = _mapper.Map<IEnumerable<ReservationDto>>(reservation);

            return PagedResult<ReservationDto>.Success(reservationDto, totalRecord, numberPage, pageSize);

        }

        public async Task<PagedResult<ReservationDto>> GetAllReservationByHotel(Guid hotelId, int numberPage, int pageSize)
        {
            var (reservations, totalRecord) = await _reservationRepository.GetPagedByHotelAsync(hotelId, numberPage, pageSize);
            var reservationDtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);

            return PagedResult<ReservationDto>.Success(reservationDtos, totalRecord, numberPage, pageSize);
        }

        public async Task<PagedResult<ReservationDto>> GetReservationByRangeDate(string startDate, string endDate, int numberPage, int pageSize)
        {
            if (!DateTime.TryParse(startDate, out DateTime start) || !DateTime.TryParse(endDate, out DateTime end))
                return PagedResult<ReservationDto>.Success([], 0, numberPage, pageSize);

            var (reservations, totalRecord) = await _reservationRepository.GetPagedByDateRangeAsync(start, end, numberPage, pageSize);

            var reservationDtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);

            return PagedResult<ReservationDto>.Success(reservationDtos, totalRecord, numberPage, pageSize);
        }
    }
}
