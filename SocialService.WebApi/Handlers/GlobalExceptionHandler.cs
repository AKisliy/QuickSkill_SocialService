using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using SocialService.Core;
using SocialService.Core.Exceptions;
using SocialService.WebApi.Dtos;

namespace SocialService.WebApi.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var errorResponse = new ErrorResponse
            {
                Message = exception.Message,
                Title = exception.GetType().Name
            };
            switch(exception)
            {
                case BadRequestException:
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case NotFoundException:
                    errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ConflictException:
                    errorResponse.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                default:
                    errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Title = "Internal service error";
                    break;
            }

            httpContext.Response.StatusCode = errorResponse.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
            return true;
        }
    }
}