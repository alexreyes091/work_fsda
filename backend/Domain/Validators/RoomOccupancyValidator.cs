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
        }
    }
}
