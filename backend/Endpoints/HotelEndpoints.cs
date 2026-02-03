using app.webapi.backoffice_viajes_altairis.Common;
using app.webapi.backoffice_viajes_altairis.Common.Enum;
using app.webapi.backoffice_viajes_altairis.Domain.Dtos;
using app.webapi.backoffice_viajes_altairis.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace app.webapi.backoffice_viajes_altairis.Endpoints
{
    public static class HotelEndpoints
    {
        public static void MapHotelEndpoints(this IEndpointRouteBuilder routes)
        {
            RouteGroupBuilder group = routes.MapGroup("api/v1/hotel").WithTags("Hotels");

            group.MapGet("/all", GetAllHotels);
            group.MapPost("/create", CreateHotel);
            group.MapGet("/{name}", GetHotelByName).WithName("GetHotelByName");
            
        }

        public static async Task<IResult> GetAllHotels(
            [FromServices] IHotelService hotelService,
            [FromQuery] int page = 1,
            [FromQuery] int size= 10
        ){
            var result = await hotelService.GetAllHotels(page, size);
            return result.ToHttpResponse();
        }

        public static async Task<IResult> CreateHotel(
            [FromServices] IHotelService hotelService,
            [FromBody] CreateHotelDto dataHotel
        ){
            var result = await hotelService.CreateHotel(dataHotel);

            if (!result.IsSuccess)
                return result.ErrorType == TypeResultResponse.VALIDATION_ERROR.ToString()
                    ? TypedResults.BadRequest(result)
                    : TypedResults.StatusCode(500);

            return TypedResults.CreatedAtRoute(result, "GetHotelByName", new { name = result.Data.Name });
        }

        public static async Task<IResult> GetHotelByName(
            [FromServices] IHotelService hotelService, 
            string name
        ){
            var result = await hotelService.GetHotelByName(name);
            return result.ToHttpResponse();
        }
    }
}
