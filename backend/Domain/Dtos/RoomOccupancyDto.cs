namespace app.webapi.backoffice_viajes_altairis.Domain.Dtos
{
    public class RoomOccupancyDto
    {
        public DateTime Date { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int OccupiedCount { get; set; }
        public int TotalRooms { get; set; } 
        public bool IsFull => OccupiedCount >= TotalRooms;
    }
}
