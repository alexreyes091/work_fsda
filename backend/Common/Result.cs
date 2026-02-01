namespace app.webapi.backoffice_viajes_altairis.Common
{
    public class Result<T> where T: class
    {
        public bool IsSuccess { get; }
        public T Data { get; }
        public string Error { get; }
        public string ErrorType { get; } // Corregido el nombre

        // Constructor protegido para permitir herencia
        protected Result(T data, bool isSuccess, string error, string errorType)
        {
            Data = data;
            IsSuccess = isSuccess;
            Error = error;
            ErrorType = errorType;
        }

        public static Result<T> Success(T data) => new(data, true, string.Empty, string.Empty);
        public static Result<T> Failure(string error, string errorType) => new(default, false, error, errorType);
    }
}
