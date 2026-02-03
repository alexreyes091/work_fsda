using app.webapi.backoffice_viajes_altairis.Common;
using app.webapi.backoffice_viajes_altairis.Common.Enum;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using app.webapi.backoffice_viajes_altairis.Domain.Models;
using app.webapi.backoffice_viajes_altairis.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace app.webapi.backoffice_viajes_altairis.Endpoints
{
    public static class RoomOccupancyEndpoints
    {
        public static void MapRoomOccupancyEndpoints(this IEndpointRouteBuilder routes)
        {
            RouteGroupBuilder group = routes.MapGroup("api/v1/room-occupancy").WithTags("Room Occupancy");

            group.MapGet("/grid/{hotelId:guid}", GetOccupancyGrid);
            group.MapPost("/initialize", InitializeStock);
        }

        public static async Task<IResult> GetOccupancyGrid(
            [FromServices] IRoomOccupancyService occupancyService,
            Guid hotelId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate
        )
        {
            var result = await occupancyService.GetOccupancyGridAsync(hotelId, startDate, endDate);

           return result.ToHttpResponse();
        }

        public static async Task<IResult> InitializeStock(
            [FromServices] IRoomOccupancyService occupancyService,
            [FromQuery] Guid roomId,
            [FromQuery] DateTime date,
            [FromQuery] int stock
        )
        {
            bool isInitialized = await occupancyService.InitializeInventoryAsync(roomId, date, stock);

            var result = isInitialized
                ? Result<string>.Success("Inventario inicializado correctamente")
                : Result<string>.Failure("No se pudo inicializar el inventario", nameof(TypeResultResponse.VALIDATION_ERROR));

            return result.ToHttpResponse();
        }
    }
}