using ASP_.NET_InvoiceManagementAuth.Database;
using ASP_.NET_InvoiceManagementAuth.Middleware;

namespace ASP_.NET_InvoiceManagementAuth.Extensions;

/// <summary>
/// Provides extension methods for <see cref="WebApplication"/> to configure the HTTP request pipeline.
/// </summary>
public static class PipelineExtensions
{
    /// <summary>
    /// Configures the middleware pipeline for the Invoice Management application.
    /// This includes Swagger UI for development, global exception handling, authentication, and routing.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
    /// <returns>The configured <see cref="WebApplication"/> instance.</returns>
    public static WebApplication UseInvoiceManagementPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice Management API v1");
                options.DisplayRequestDuration();
                options.EnableFilter();
                options.EnableDeepLinking();
                options.EnableTryItOutByDefault();
                options.EnablePersistAuthorization();
            });
        }

        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            RoleSeeder.SeedRolesAsync(scope.ServiceProvider).GetAwaiter().GetResult();
        }
        app.MapControllers();

        return app;
    }

}


