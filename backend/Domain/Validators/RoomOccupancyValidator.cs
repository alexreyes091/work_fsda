using app.webapi.backoffice_viajes_altairis.Data.Interfaces;
using app.webapi.backoffice_viajes_altairis.Domain.Models;
using FluentValidation;

namespace app.webapi.backoffice_viajes_altairis.Domain.Validators
{
    public class RoomOccupancyValidator : AbstractValidator<RoomOccupancy>
    {
        private readonly IRoomRepository _roomRepository;
        public RoomOccupancyValidator(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("La fecha de ocupación es requerida.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("La fecha de ocupación no puede ser antes de la fecha actual.");

            //RuleFor(x => x).MustAsync(async (dto, cancellation) => {
            //    var maxCapacity = (await _roomRepository.GetByIdAsync(dto.RoomId))?.Capacity;
            //    var currentOccupancy = await _occupancyRepo.GetMaxOccupancyInRangeAsync(dto.RoomId, dto.CheckIn, dto.CheckOut);

            //    return (maxCapacity - currentOccupancy) > 0;
            //}).WithMessage("Lo sentimos, ya no hay disponibilidad para las fechas seleccionadas.");
        }
    }
}
