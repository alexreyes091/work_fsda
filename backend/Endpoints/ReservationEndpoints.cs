namespace app.webapi.backoffice_viajes_altairis.Endpoints
{
    public static class ReservationEndpoints 
    {
        public static void MapReservationEndpoints(this IEndpointRouteBuilder routes)
        {
            RouteGroupBuilder group = routes.MapGroup("api/v1/hotel").WithTags("Reservations");

            //TODO: Agregar endpoints para reservaciones
        }
    }
}
