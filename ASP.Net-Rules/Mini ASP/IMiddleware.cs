using System.Net;
namespace ASP.Net_Rules.Mini_ASP;
public delegate void HttpHandler(HttpListenerContext context);

public interface IMiddleware
{
    public HttpHandler Next { get; set; }
    public void Handle(HttpListenerContext context);

}
