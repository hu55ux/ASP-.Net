using System.Net;

namespace ASP.Net_Rules;

public class LoggerMiddleware : IMiddleware
{
    public IMiddleware? Next { get; set; }

    public void Handle(HttpListenerContext context)
    {
        Console.WriteLine($"""
            {context.Request.HttpMethod}
            {context.Request.RawUrl}
            {context.Request.RemoteEndPoint}

            """);
        Next.Handle(context);
    }
}
