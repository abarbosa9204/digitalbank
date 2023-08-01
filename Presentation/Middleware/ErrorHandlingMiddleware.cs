using System.Net;
using System.Text.Json;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ErrorHandlingMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";

            ErrorResponse errorResponse;
            int statusCode;

            if (ex is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse = new ErrorResponse { Code = statusCode, Message = "Acceso no autorizado." };
            }
            else if (ex is CustomException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse { Code = statusCode, Message = "Error: " + ex.Message };
            }
            else if (ex is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new ErrorResponse { Code = statusCode, Message = "Recurso no encontrado." };
            }
            else if (ex is BadRequestException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse { Code = statusCode, Message = "Solicitud incorrecta." };
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse = new ErrorResponse { Code = statusCode, Message = "Ha ocurrido un error en el servidor. Por favor, intente nuevamente más tarde." };
            }

            errorResponse.ExceptionMessage = ex.Message;

            var errorJson = JsonSerializer.Serialize(errorResponse);

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(errorJson);
        }
    }
}

public class ErrorResponse
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public string? ExceptionMessage { get; set; }
}

public class CustomException : Exception
{
    public CustomException(string message) : base(message)
    {
        throw new CustomException("Error no especificado.");
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
