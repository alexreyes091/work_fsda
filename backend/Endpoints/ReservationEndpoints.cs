using app.webapi.backoffice_viajes_altairis.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace app.webapi.backoffice_viajes_altairis.Endpoints
{
    public static class ReservationEndpoints 
    {
        public static void MapReservationEndpoints(this IEndpointRouteBuilder routes)
        {
            RouteGroupBuilder group = routes.MapGroup("api/v1/reservation").WithTags("Reservations");

            group.MapGet("/all", GetAllReservation);
            group.MapGet("/byHotelId/{id:guid}", GetAllReservationByHotel);
            group.MapGet("/byRange", GetAllReservationByRangeDate);
        }

        private static async Task<IResult> GetAllReservationByRangeDate(
            [FromServices] IReservationService reservationService,
            [FromQuery] string startDate,
            [FromQuery] string endDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5
        ){
            var result = await reservationService.GetReservationByRangeDate(startDate, endDate, pageNumber, pageSize);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }

        private static async Task<IResult> GetAllReservationByHotel(
            [FromServices] IReservationService reservationService,
            Guid id,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5
        ){
            var result = await reservationService.GetAllReservationByHotel(id, pageNumber, pageSize);

            return result.IsSuccess
                    ? TypedResults.Ok(result)
                    : TypedResults.BadRequest(result);
        }

        public static async Task<IResult> GetAllReservation(
            [FromServices] IReservationService reservationService,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5
        ){
            var result = await reservationService.GetAllReservation(pageNumber, pageSize);

            return result.IsSuccess
                    ? TypedResults.Ok(result)
                    : TypedResults.BadRequest(result);
        }
    }
}
