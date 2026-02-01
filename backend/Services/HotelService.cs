using app.webapi.backoffice_viajes_altairis.Common;
using app.webapi.backoffice_viajes_altairis.Common.Enum;
using app.webapi.backoffice_viajes_altairis.Data.Interfaces;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using app.webapi.backoffice_viajes_altairis.Domain.Models;
using app.webapi.backoffice_viajes_altairis.Domain.Validators;
using app.webapi.backoffice_viajes_altairis.Services.Interfaces;
using FluentValidation;
using MapsterMapper;

namespace app.webapi.backoffice_viajes_altairis.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly HotelValidator _hotelValidator;
        private readonly IMapper _mapper;

        public HotelService(IHotelRepository hotelRepository, HotelValidator hotelValidator, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _hotelValidator = hotelValidator;
            _mapper = mapper;
        }

        public async Task<Result<HotelDto>> GetHotelByName(string name)
        {
            Hotel? hotel = await _hotelRepository.GetByNameAsync(name);
            
            if (hotel == null)
                return Result<HotelDto>.Failure("Hotel no encontrado.", TypeResultResponse.NOT_FOUND.ToString());

            var hotelDto = _mapper.Map<HotelDto>(hotel);
            return Result<HotelDto>.Success(hotelDto);
        }

        public async Task<Result<HotelDto>> CreateHotel(CreateHotelDto dataHotel)
        {
            Hotel hotel = _mapper.Map<Hotel>(dataHotel);

            var validationResult = await _hotelValidator.ValidateAsync(hotel);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<HotelDto>.Failure( $"Error de validación: {errors}", TypeResultResponse.VALIDATION_ERROR.ToString());
            }

            if (await _hotelRepository.ExistAsync(hotel.Name))
                return Result<HotelDto>.Failure("El nombre del Hotel ya existe.",TypeResultResponse.VALIDATION_ERROR.ToString());
            bool isSaved = await _hotelRepository.CreateAsync(hotel);

            return isSaved
                ? Result<HotelDto>.Success(_mapper.Map<HotelDto>(hotel))
                : Result<HotelDto>.Failure("No se pudo crear el Hotel.", TypeResultResponse.ERROR_EXCEPTION.ToString());
        }

        public async Task<PagedResult<HotelDto>> GetAllHotels(int numberPage, int pageSize)
        {
            var (hotels, totalRecords) = await _hotelRepository.GetPagedAsync(numberPage, pageSize);

            IEnumerable<HotelDto> hotelDtos = _mapper.Map<IEnumerable<HotelDto>>(hotels);

            return PagedResult<HotelDto>.Success(hotelDtos, totalRecords, numberPage, pageSize);
        }
    }
}
