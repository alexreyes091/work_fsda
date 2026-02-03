using app.webapi.backoffice_viajes_altairis.Domain.Models;

namespace app.webapi.backoffice_viajes_altairis.Domain.Dtos
{
    public class CreateReservationDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string GuestFullName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}