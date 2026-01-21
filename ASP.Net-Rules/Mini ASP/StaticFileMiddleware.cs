using System.Net;

namespace ASP.Net_Rules.Mini_ASP;

public class StaticFileMiddleware : IMiddleware
{
    public HttpHandler Next { get; set; }

    public void Handle(HttpListenerContext context)
    {
        var basePath = @"..\..\..\wwwroot";
        if (Path.HasExtension(context.Request.RawUrl))
        {
            try
            {
                var fileName = context.Request.RawUrl.Substring(1);
                var path = $"{basePath}{fileName}";
                var bytes = File.ReadAllBytes(path);
                if (Path.GetExtension(path) == ".html")
                {
                    context.Response.AddHeader("Content-Type", "text/html");
                }
                else if (Path.GetExtension(path) == ".png")
                {
                    context.Response.AddHeader("Content-Type", "image/png");
                }
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 404;
                context.Response.StatusDescription = "File Not Found";
                var bytes = File.ReadAllBytes($"{basePath}404.hmtl");
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
        }
        else
        {
            Next.Invoke(context);
        }
        context.Response.Close();
    }
}
