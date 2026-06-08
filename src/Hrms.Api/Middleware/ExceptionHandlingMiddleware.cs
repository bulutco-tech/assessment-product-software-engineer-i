using System.Net;
using Hrms.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BusinessRuleException exception)
        {
            logger.LogWarning(
                exception,
                "BusinessRuleViolation {ErrorCode} {Path}",
                exception.ErrorCode,
                context.Request.Path.Value);

            await WriteProblemAsync(
                context,
                HttpStatusCode.BadRequest,
                "Business rule violation",
                exception.Message,
                exception.ErrorCode);
        }
        catch (NotFoundException exception)
        {
            logger.LogWarning(
                exception,
                "ResourceNotFound {ResourceName} {ResourceId} {Path}",
                exception.ResourceName,
                exception.ResourceId,
                context.Request.Path.Value);

            await WriteProblemAsync(
                context,
                HttpStatusCode.NotFound,
                "Resource not found",
                exception.Message);
        }
        catch (NotImplementedException exception)
        {
            logger.LogInformation(
                exception,
                "AssessmentSkeletonEndpointCalled {Path}",
                context.Request.Path.Value);

            await WriteProblemAsync(
                context,
                HttpStatusCode.NotImplemented,
                "Assessment skeleton not implemented",
                exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "UnhandledException {Path}",
                context.Request.Path.Value);

            await WriteProblemAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Unexpected server error",
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string title,
        string detail,
        string? errorCode = null)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        if (!string.IsNullOrWhiteSpace(errorCode))
        {
            problem.Extensions["errorCode"] = errorCode;
        }

        await context.Response.WriteAsJsonAsync(problem);
    }
}
