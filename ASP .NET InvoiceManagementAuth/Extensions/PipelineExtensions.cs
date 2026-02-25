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
        // 1. Development Environment Configuration
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Sets the Swagger JSON endpoint
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice Management API v1");

                // Serves Swagger UI at the application's root (e.g., http://localhost:5000/)
                options.RoutePrefix = string.Empty;

                // UI Enhancements
                options.DisplayRequestDuration();
                options.EnableFilter();
                options.EnableDeepLinking();
                options.EnableTryItOutByDefault();
                options.EnablePersistAuthorization(); // Keeps JWT token after page refresh
            });
        }

        // 2. Global Error Handling
        /// <summary>
        /// Custom middleware to catch all unhandled exceptions and return a consistent JSON response.
        /// This should be placed early in the pipeline to catch errors from subsequent layers.
        /// </summary>
        app.UseMiddleware<GlobalExceptionMiddleware>();

        // 3. Security Middlewares
        // Order is critical here: Authentication must come before Authorization.
        app.UseAuthentication();
        app.UseAuthorization();

        // 4. Endpoint Mapping
        // Maps Controller actions to their respective routes.
        app.MapControllers();

        return app;
    }
}