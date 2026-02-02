using app.webapi.backoffice_viajes_altairis.Common.Enum;

namespace app.webapi.backoffice_viajes_altairis.Common
{
    public static class ResultExtensions
    {
        public static IResult ToHttpResponse<T>(this Result<T> result) where T : class
        {
            if (result.IsSuccess)
                return TypedResults.Ok(result);

            return result.ErrorType switch
            {
                nameof(TypeResultResponse.VALIDATION_ERROR) => TypedResults.BadRequest(result),
                nameof(TypeResultResponse.NOT_FOUND) => TypedResults.NotFound(result),
                nameof(TypeResultResponse.ERROR_DB_CONNECTION) => TypedResults.StatusCode(StatusCodes.Status503ServiceUnavailable),
                _ => TypedResults.StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
    }
}