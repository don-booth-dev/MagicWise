using System.Net;

namespace MagicWise.Integrations.V1.Models.Responses;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public T? Data { get; set; }
    public ApiErrorDto? Error { get; set; }

    public static ApiResponse<T> Success(T? data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Data = data
        };
    }

    public static ApiResponse<T> Failure(ApiErrorDto error, HttpStatusCode statusCode)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Error = error
        };
    }
}
