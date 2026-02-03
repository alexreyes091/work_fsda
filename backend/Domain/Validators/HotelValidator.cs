using app.webapi.backoffice_viajes_altairis.Domain.Models;
using FluentValidation;

namespace app.webapi.backoffice_viajes_altairis.Domain.Validators
{
    public class HotelValidator : AbstractValidator<Hotel>
    {
        public HotelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del hotel es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del hotel no puede exceder los 100 caracteres.");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("La dirección del hotel es obligatoria.")
                .MaximumLength(200).WithMessage("La dirección del hotel no puede exceder los 200 caracteres.");
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("La ciudad del hotel es obligatoria.")
                .MaximumLength(100).WithMessage("La ciudad del hotel no puede exceder los 100 caracteres.");
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("El país del hotel es obligatorio.")
                .MaximumLength(100).WithMessage("El país del hotel no puede exceder los 100 caracteres.");
            RuleFor(x => x.Stars)
                .InclusiveBetween(1, 5).WithMessage("Las estrellas del hotel deben estar entre 1 y 5.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("El número de teléfono del hotel es obligatorio.")
                .MaximumLength(20).WithMessage("El número de teléfono del hotel no puede exceder los 20 caracteres.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico del hotel es obligatorio.")
                .EmailAddress().WithMessage("El correo electrónico del hotel no es válido.")
                .MaximumLength(100).WithMessage("El correo electrónico del hotel no puede exceder los 100 caracteres.");
            RuleFor(x => x.TotalRooms)
                .GreaterThanOrEqualTo(0).WithMessage("El total de habitaciones del hotel no puede ser menor a cero.");
        }
    }
}
