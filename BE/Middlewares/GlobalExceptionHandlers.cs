using System;
using BE.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BE.Middlewares;


public class GlobalExceptionHandlers(ILogger<GlobalExceptionHandlers> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();
        problemDetails.Instance = httpContext.Request.Path;

         if (exception is BaseException baseException)
        {
            problemDetails.Title = baseException.Message;
            httpContext.Response.StatusCode = (int)baseException.StatusCode;
        }
        else
        {
            problemDetails.Title = "An unexpected error occurred.";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);

        problemDetails.Status = httpContext.Response.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }
}