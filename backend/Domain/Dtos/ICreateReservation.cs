using app.webapi.backoffice_viajes_altairis.Domain.Models;

namespace app.webapi.backoffice_viajes_altairis.Domain.Dtos
{
    public interface ICreateReservation
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string GuestFullName { get; set; }
        public string GuestEmail { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; }
        public Room Room { get; set; }
    }
}