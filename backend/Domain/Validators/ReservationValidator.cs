using app.webapi.backoffice_viajes_altairis.Domain.Models;
using FluentValidation;

namespace app.webapi.backoffice_viajes_altairis.Domain.Validators
{
    public class ReservationValidator : AbstractValidator<Reservation>
    {
        public ReservationValidator() 
        {
            RuleFor(reservation => reservation.GuestFullName)
                .NotEmpty().WithMessage("El nombre del invitado es requerido.")
                .MaximumLength(200).WithMessage("El nombre del invitado no puede exceder los 200 caracteres.");
            RuleFor(reservation => reservation.GuestEmail)
                .NotEmpty().WithMessage("El email es requerido.")
                .EmailAddress().WithMessage("Debe de ingresar un email válido.")
                .MaximumLength(100).WithMessage("El email no puede exceder los 100 caracteres.");
            RuleFor(reservation => reservation.PhoneNumber)
                .NotEmpty().WithMessage("El numero de teléfono es requerido.")
                .MaximumLength(15).WithMessage("El numero de teléfono no puede exceder los 15 caracteres");
            RuleFor(reservation => reservation.CheckIn)
                .NotEmpty().WithMessage("La fecha de ingreso es requerido")
                .LessThan(reservation => reservation.CheckOut).WithMessage("La fecha de ingreso debe de ser anterior a la fecha de salida.");
            RuleFor(reservation => reservation.CheckOut)
                .NotEmpty().WithMessage("La fecha de salida es requerida")
                .GreaterThan(reservation => reservation.CheckIn).WithMessage("La fecha de salida debe de ser posterior a la fecha de entrada");
            RuleFor(reservation => reservation.Price)
                .GreaterThan(0).WithMessage("El precio debe de ser mayor a cero.");


        }
    }
}
