using System;
using System.Net;

namespace BE.Exceptions;

public class BaseException : Exception
{
    public HttpStatusCode StatusCode { get; }
public BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    : base(message)
{
    StatusCode = statusCode;
}
}
