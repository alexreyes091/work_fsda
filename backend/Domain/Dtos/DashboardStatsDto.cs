namespace app.webapi.backoffice_viajes_altairis.Domain.Dtos
{
    public class DashboardStatsDto
    {
        public int TotalHotels { get; set; }
        public int TotalRooms { get; set; }
        public int ActiveReservations { get; set; }
        public int OccupancyRate { get; set; }
        public int CheckInsToday { get; set; }
        public int CheckOutsToday { get; set; }
        public int PendingReservations { get; set; }
        public decimal MonthlyRevenue { get; set; }
    }
}
