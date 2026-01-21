namespace ASP.Net_Rules.Mini_ASP;

class MiddlewareBuilder
{
    private Stack<Type> _middlewares = new();
    public void Use<T>() where T : IMiddleware
    {
        _middlewares.Push(typeof(T));
    }
    public HttpHandler Build()
    {
        HttpHandler handler = context => context.Response.Close();
        while (_middlewares.Count > 0)
        {
            var middleware = _middlewares.Pop();
            IMiddleware middleware1 = Activator.CreateInstance(middleware) as IMiddleware;
            middleware1.Next = handler;
            handler = middleware1.Handle;
        }
        return handler;
    }
}