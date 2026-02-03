using app.webapi.backoffice_viajes_altairis.Domain.Models;

namespace app.webapi.backoffice_viajes_altairis.Domain.Dtos
{
    public class RoomDto
    {
        public Guid HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string> Services { get; set; } = [.. string.Empty.Split(',')];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
