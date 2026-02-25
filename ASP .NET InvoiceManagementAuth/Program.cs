using ASP_.NET_InvoiceManagementAuth.Extensions;

/// <summary>
/// The main entry point for the Invoice Management Authentication API.
/// This class initializes the application builder, registers core services, 
/// and configures the middleware request pipeline.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// --- Service Registration ---
/// <summary>
/// Configures dependency injection for the application.
/// Includes Swagger for documentation, FluentValidation for data integrity,
/// Identity for user management, and custom database contexts.
/// </summary>
builder.Services
    .AddSwagger()
    .AddFluentValidation()
    .AddInvoiceManagmentDBContext(builder.Configuration)
    .AddIdentityAndDb(builder.Configuration)
    .AddAuthenticationAndAuthorization(builder.Configuration)
    .AddAutoMapperAndOthers();

var app = builder.Build();

// --- Middleware Pipeline ---
/// <summary>
/// Defines the HTTP request processing pipeline.
/// Encapsulates routing, authentication, authorization, and custom exception handling
/// via the centralized <see cref="UseInvoiceManagementPipeline"/> extension.
/// </summary>
app.UseInvoiceManagementPipeline();

/// <summary>
/// Starts the web application and begins listening for incoming HTTP requests.
/// </summary>
app.Run();