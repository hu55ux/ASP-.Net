using ASP_.NET_InvoiceManagementAuth.Middleware;

namespace ASP_.NET_InvoiceManagementAuth.Extensions;

public static class PipelineExtensions
{
    public static WebApplication UseInvoiceManagementPipeline(
        this WebApplication app
        )
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskFlow API v1");
                    options.RoutePrefix = string.Empty;
                    options.DisplayRequestDuration();
                    options.EnableFilter();
                    options.EnableDeepLinking();
                    options.EnableTryItOutByDefault();
                    options.EnablePersistAuthorization();
                }
                );
        }
        app.UseMiddleware<GlobalExceptionMiddleware>();


        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

}
