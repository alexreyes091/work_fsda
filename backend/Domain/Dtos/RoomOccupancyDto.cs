namespace app.webapi.backoffice_viajes_altairis.Domain.Dtos
{
    public class RoomOccupancyDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int OccupiedCount { get; set; }
    }
}
