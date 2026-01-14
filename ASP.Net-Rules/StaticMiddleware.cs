namespace ASP.Net_Rules;
using System.Net;

public class StaticMiddleware : IMiddleware
{
    public IMiddleware? Next { get; set; }

    public void Handle(HttpListenerContext context)
    {
        if (Path.HasExtension(context.Request.RawUrl))
        {
            var 
            try
            {
                var fileName = context.Request.RawUrl.Substring(1);
                var path = 
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

