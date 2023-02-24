using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using middlewaredenemev6.Middlewares.Class;
using middlewaredenemev6.Middlewares.Logging;

namespace middlewaredenemev6.Middlewares.Middleware;


[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    private readonly ILoggingService _logger;

    public ErrorsController(ILoggingService logger)
    {
        _logger = logger;
    }

    [Route("error")]
    public ErrorResponse Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context.Error;

        // Write error
        _logger.exception = "Message : " + context.Error.Message + " | StackTrace : " + context.Error.StackTrace;

        ErrorResponse mr = new ErrorResponse(context.Error);
        mr.Message = "Error Message";
        mr.StackTrace = "";
        mr.Type = "ERROR";

        Response.StatusCode = 999;

        return mr;
    }
}