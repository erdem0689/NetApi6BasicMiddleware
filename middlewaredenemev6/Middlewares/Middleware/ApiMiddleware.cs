using System.Globalization;
using System.Net;
using System.Text;
using middlewaredenemev6.Middlewares.Class;
using middlewaredenemev6.Middlewares.Logging;

namespace middlewaredenemev6.Middlewares.Middleware;

public class ApiMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _logger;

    public ApiMiddleware(RequestDelegate next, ILoggingService logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        HttpRequest request = context.Request;
        _logger.requestId = Guid.NewGuid().ToString();
        _logger.hostIp = Dns.GetHostName();
        _logger.methodName = request.Path.Value;
        _logger.header = FormatHeaders(request.Headers);
        var req = FormatQueries(request.QueryString.ToString());
        string res = "";

        try
        {
            string requestBody;
            context.Request.EnableBuffering();
            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBody = await reader.ReadToEndAsync();
            }
            _logger.request = requestBody;
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 300;
        }
        finally
        {
            context.Request.Body.Position = 0;
        }

        Stream originalBody = context.Response.Body;

        try
        {
            using (var memStream = new MemoryStream())
            {
                context.Response.Body = memStream;
                await _next(context);
                memStream.Seek(0, SeekOrigin.Begin);
                string responseBody = new StreamReader(memStream).ReadToEnd();
                res = responseBody;
                memStream.Position = 0;
                if (context.Response.StatusCode != 200 && context.Response.StatusCode != 999)
                {
                    var tempStatusCode = context.Response.StatusCode;
                    ErrorResponse errorResponse = new ErrorResponse();
                    errorResponse.Type = "Error";
                    errorResponse.Message = "Error Message";

                    _logger.exception = res;

                    context.Response.Clear();
                    context.Response.StatusCode = tempStatusCode;
                    await context.Response.WriteAsJsonAsync(errorResponse);
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                }
                else
                    _logger.response = res;

                await memStream.CopyToAsync(originalBody);

            }
        }
        // catch (Exception ex)
        // {
        //     Console.WriteLine(ex.Message);
        //     throw ex;
        //     context.Response.StatusCode = 500;
        // }
        finally
        {
            context.Response.Body = originalBody;
        }

        if (context.Response.StatusCode == 200)
        {
            _logger.Log();
        }
    }

    private List<KeyValuePair<string, string>> FormatQueries(string queryString)
    {
        List<KeyValuePair<string, string>> pairs =
             new List<KeyValuePair<string, string>>();
        string key, value;
        foreach (var query in queryString.TrimStart('?').Split("&"))
        {
            var items = query.Split("=");
            key = items.Count() >= 1 ? items[0] : string.Empty;
            value = items.Count() >= 2 ? items[1] : string.Empty;
            if (!String.IsNullOrEmpty(key))
            {
                pairs.Add(new KeyValuePair<string, string>(key, value));
            }
        }
        return pairs;
    }

    private string FormatHeaders(IHeaderDictionary headers)
    {
        string headerResult = "";
        Dictionary<string, string> pairs = new Dictionary<string, string>();
        foreach (var header in headers)
        {
            headerResult += header.Key + ": " + header.Value + " | ";
        }
        return headerResult;
    }


}



