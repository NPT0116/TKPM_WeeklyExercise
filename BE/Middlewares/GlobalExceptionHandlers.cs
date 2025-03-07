using System;
using System.Threading;
using System.Threading.Tasks;
using BE.Exceptions;
using BE.Utils; // Response<T> class is defined here
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BE.Middlewares
{
    public class GlobalExceptionHandlers(ILogger<GlobalExceptionHandlers> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            Response<object> responseObj;

            if (exception is BaseException baseException)
            {
                responseObj = new Response<object>(
                    data: null,
                    message: baseException.Message,
                    success: false)
                {
                    Errors = new[] { baseException.Message }
                };

                httpContext.Response.StatusCode = (int)baseException.StatusCode;
                Console.WriteLine("Error: {0}", baseException.Message);
            }
            else
            {
                responseObj = new Response<object>(
                    data: null,
                    message: "An unexpected error occurred.",
                    success: false)
                {
                    Errors = new[] { "An unexpected error occurred." }
                };

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            logger.LogError("{Error}", responseObj.Message);

            await httpContext.Response.WriteAsJsonAsync(responseObj, cancellationToken)
                                 .ConfigureAwait(false);
            return true;
        }
    }
}
