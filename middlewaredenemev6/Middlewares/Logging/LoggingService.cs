
using Newtonsoft.Json;

namespace middlewaredenemev6.Middlewares.Logging;

public class LoggingService : ILoggingService
{
    public string requestId { get; set; }
    public string hostIp { get; set; }
    public string methodName { get; set; }
    public string header { get; set; }
    public string request { get; set; }
    public string response { get; set; }
    public string exception { get; set; }
    public string requestDate { get; set; }
    public string responseDate { get; set; }
    public long time { get; set; }

    public void Log()
    {
        LoggingService s = (LoggingService)this.MemberwiseClone();
        if (s.methodName.IndexOf("api/") != -1)
        {
            var json = JsonConvert.SerializeObject(s);
            Console.WriteLine(json);
        }
    }
}

public interface ILoggingService
{

    public string requestId { get; set; }
    public string hostIp { get; set; }
    public string methodName { get; set; }
    public string header { get; set; }
    public string request { get; set; }
    public string response { get; set; }
    public string exception { get; set; }
    public string requestDate { get; set; }
    public string responseDate { get; set; }
    public long time { get; set; }
    public void Log();
}