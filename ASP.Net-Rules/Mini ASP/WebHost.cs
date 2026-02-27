using System.Net;

namespace ASP.Net_Rules.Mini_ASP;

public class WebHost
{
    private int _port;
    private HttpHandler _handler;
    HttpListener _listener;
    private MiddlewareBuilder _builder = new();

    public WebHost(int port)
    {
        _port = port;
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{_port}/");
    }
    public void Run()
    {
        _listener.Start();
        Console.WriteLine($"Listening on port {_port}...");
        while (true)
        {
            HttpListenerContext context = _listener.GetContext();
            Task.Run(() => HandlerRequest(context));
        }
    }

    private void HandlerRequest(HttpListenerContext context)
    {
        _handler.Invoke(context);
    }

}
