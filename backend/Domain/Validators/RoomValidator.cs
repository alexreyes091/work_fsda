using app.webapi.backoffice_viajes_altairis.Domain.Models;
using FluentValidation;

namespace app.webapi.backoffice_viajes_altairis.Domain.Validators
{
    public class RoomValidator : AbstractValidator<Room>
    {
        public RoomValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la habitación es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre de la habitación no puede exceder los 100 caracteres.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción de la habitación es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción de la habitación no puede exceder los 500 caracteres.");
            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("La capacidad de la habitación debe ser mayor a cero.");
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("La cantidad de habitaciones no puede ser menor a cero.");
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("El precio de la habitación no puede ser menor a cero.");
            RuleFor(x => x.Services)
                .NotNull().WithMessage("Debe de existir al menos un servicio para la habitación.");
        }
    }
}
