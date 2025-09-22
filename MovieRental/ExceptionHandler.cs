using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace MovieRental
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ValidationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            if (exception is Exceptions.NotFoundException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            await httpContext.Response.WriteAsync(exception.Message, cancellationToken);
            return true;
        }
    }
}