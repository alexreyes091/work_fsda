using app.webapi.backoffice_viajes_altairis.Domain.Enums;
using app.webapi.backoffice_viajes_altairis.Domain.Models;

namespace app.webapi.backoffice_viajes_altairis.Domain.Dtos
{
    public class ReservationDto
    {
        public Guid RoomId { get; set; }
        public string GuestFullName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; } = DateTime.UtcNow;
        public DateTime CheckOut { get; set; }
        public int Nights => (CheckOut - CheckIn).Days;
        public decimal Price { get; set; }
        public string Notes { get; set; } = string.Empty;
        public TypeStatusReservation StatusReservation { get; set; }
    }
}
