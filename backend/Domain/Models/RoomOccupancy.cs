namespace app.webapi.backoffice_viajes_altairis.Domain.Models
{
    public class RoomOccupancy
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public DateTime Date { get; set; }
        public int OccupiedCount { get; set; }
    }
}
