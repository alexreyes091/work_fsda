using app.webapi.backoffice_viajes_altairis.Domain.Enums;

namespace app.webapi.backoffice_viajes_altairis.Domain.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string GuestFullName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; } = DateTime.UtcNow;
        public DateTime CheckOut { get; set; }
        public int Nights => (CheckOut - CheckIn).Days;
        public decimal Price { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public TypeStatusReservation StatusReservation { get; set; } = TypeStatusReservation.PENDING;
        public Room Room { get; set; } = null!;

    }
}
