using app.webapi.backoffice_viajes_altairis.Common;
using app.webapi.backoffice_viajes_altairis.Common.Enum;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using app.webapi.backoffice_viajes_altairis.Services;
using app.webapi.backoffice_viajes_altairis.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace app.webapi.backoffice_viajes_altairis.Endpoints
{
    public static class RoomEndpoint 
    {
        public static void MapRoomEndpoints(this IEndpointRouteBuilder routes)
        {
            RouteGroupBuilder group = routes.MapGroup("api/v1/room").WithTags("Rooms");

            group.MapGet("/all", GetAllRooms);
            group.MapPost("/create", CreateRoom).WithName("GetRoomByName");
            group.MapGet("/byHotelName/{name}", GetAllRoomsByHotelName);
        }

        public static async Task<IResult> GetAllRooms(
            [FromServices] IRoomService roomService,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5
        ){
            var result = await roomService.GetAllRooms(pageNumber, pageSize);
            return result.ToHttpResponse();
        }

        public static async Task<IResult> CreateRoom(
            [FromServices] IRoomService roomService,
            [FromBody] CreateRoomDto roomDto
        ){
            var result = await roomService.CreateRoom(roomDto);

            if (!result.IsSuccess) return result.ToHttpResponse();

            return TypedResults.CreatedAtRoute(result, "GetRoomByName", new { name = result.Data.Name });
        }

        public static async Task<IResult> GetAllRoomsByHotelName(
            [FromServices] IRoomService roomService,
            string name
        ){
            var result = await roomService.GetAllRoomsByHotelName(name);
            return result.ToHttpResponse();
        }
    }
}
