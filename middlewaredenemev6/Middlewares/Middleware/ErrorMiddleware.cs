using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using middlewaredenemev6.Middlewares.Class;

namespace middlewaredenemev6.Middlewares.Middleware;


[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    [Route("error")]
    public ErrorResponse Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context.Error;

        // Write error
        Console.WriteLine("ErrorMiddleware Message: " + context.Error.Message);
        Console.WriteLine("ErrorMiddleware STrace: " + context.Error.StackTrace);

        ErrorResponse mr = new ErrorResponse(context.Error);
        mr.Message = "Error Message";
        mr.StackTrace = "";
        mr.Type = "ERROR";

        Response.StatusCode = 999;

        return mr;
    }
}