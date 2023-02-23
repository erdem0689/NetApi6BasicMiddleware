using System.Diagnostics;
using middlewaredenemev6.Middlewares.Logging;

namespace middlewaredenemev6.Middlewares.Middleware;

public class TimeLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _logger;

    public TimeLoggingMiddleware(RequestDelegate next, ILoggingService logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        await _next(context);

        watch.Stop();
        // TODO : Zaman Logla
        _logger.time = watch.ElapsedMilliseconds;
        _logger.Log();
        //Console.WriteLine("TIMEMEMEMEMEMEME: " + watch.ElapsedMilliseconds + " milliseconds.");
    }
}