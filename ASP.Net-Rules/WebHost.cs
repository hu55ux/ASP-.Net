using System.Net;

class WebHost
{
    int port;
    string pathBase = @"..\..\..\";
    HttpListener listener;
    public WebHost(int port)
    {
        this.port = port;
    }
    public void Run()
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}/");
        listener.Start();
        Console.WriteLine($"Server is running on {port}/");

        while (true)
        {
            var context = listener.GetContext();
            Task.Run(() => HandleRequest(context));
        }
    }

    private void HandleRequest(HttpListenerContext context)
    {
        var url = context.Request.RawUrl;
        var path = $@"{pathBase}{url.Split("/").Last()}";
        var response = context.Response;
        StreamWriter writer = new StreamWriter(response.OutputStream);
        try
        {
            var src = File.ReadAllText(path);
            writer.WriteLine(src);
        }
        catch (Exception)
        {
            var src = File.ReadAllText($@"{pathBase}404.html");
            throw;
        }
        finally
        {
            writer.Close();
        }
    }
}