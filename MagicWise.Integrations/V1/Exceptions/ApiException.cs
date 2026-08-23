using System;
using System.Net;
using MagicWise.Integrations.V1.Models.Responses;

namespace MagicWise.Integrations.V1;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public ApiErrorDto? Error { get; }

    public ApiException(string message, HttpStatusCode statusCode, ApiErrorDto? error = null)
        : base(message)
    {
        StatusCode = statusCode;
        Error = error;
    }
}