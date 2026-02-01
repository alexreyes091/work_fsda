namespace app.webapi.backoffice_viajes_altairis.Domain.Models
{
    public class Hotel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Stars { get; set; } = 0;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalRooms { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public List<Room> Rooms { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
