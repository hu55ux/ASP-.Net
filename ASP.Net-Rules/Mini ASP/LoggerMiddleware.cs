using System.Net;

namespace ASP.Net_Rules.Mini_ASP;

public class LoggerMiddleware : IMiddleware
{
    public HttpHandler Next { get; set; }

    public void Handle(HttpListenerContext context)
    {
        Console.WriteLine($"""
            {context.Request.HttpMethod}
            {context.Request.RawUrl}
            {context.Request.RemoteEndPoint}
            """);
        Next.Invoke(context);
    }
}
