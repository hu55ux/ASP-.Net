using System.Text;
using ASP_.NET_InvoiceManagementAuth.Config;
using ASP_.NET_InvoiceManagementAuth.Database;
using ASP_.NET_InvoiceManagementAuth.Mapping;
using ASP_.NET_InvoiceManagementAuth.Models;
using ASP_.NET_InvoiceManagementAuth.Services;
using ASP_.NET_InvoiceManagementAuth.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ASP_.NET_InvoiceManagementAuth.Extensions;

/// <summary>
/// Static class containing extension methods for <see cref="IServiceCollection"/> 
/// to facilitate a modular and clean service registration process in Program.cs.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures Swagger/OpenAPI generation. 
    /// Sets up API metadata, XML documentation integration, and JWT Bearer security definitions for the UI.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The same service collection for method chaining.</returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Invoice Management System",
                Description = "A professional API for managing customer invoices and secure authentication.",
                Contact = new OpenApiContact
                {
                    Name = "Huseyn Sebziyev",
                    Email = "hsbziyev@gmail.com"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            // Integrates XML comments to show documentation on Swagger UI
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            // Defines the 'Bearer' security scheme for JWT
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }

    /// <summary>
    /// Registers the primary Database Context for the application.
    /// Configures the connection to SQL Server using the 'DefaultConnectionString' from appsettings.json.
    /// </summary>
    public static IServiceCollection AddInvoiceManagmentDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        services.AddDbContext<InvoiceManagmentDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }

    /// <summary>
    /// Configures Microsoft Identity for user and role management.
    /// Defines password policies and links the identity system to the Entity Framework store.
    /// </summary>
    public static IServiceCollection AddIdentityAndDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfig>(configuration.GetSection(JwtConfig.SectionName));

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            // Development-friendly password settings
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddEntityFrameworkStores<InvoiceManagmentDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }

    /// <summary>
    /// Configures JWT Authentication and defines the parameters for token validation.
    /// Sets ClockSkew to zero to ensure strict token expiration enforcement.
    /// </summary>
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = new JwtConfig();
        configuration.GetSection(JwtConfig.SectionName).Bind(jwtConfig);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey!)),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();
        return services;
    }

    /// <summary>
    /// Enables FluentValidation for automatic model state validation.
    /// Scans the assembly to register all validator classes automatically.
    /// </summary>
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<Program>();
        return services;
    }

    /// <summary>
    /// Registers AutoMapper profiles and Business Logic Services.
    /// Uses Scoped lifetime for services to ensure data consistency per HTTP request.
    /// </summary>
    public static IServiceCollection AddAutoMapperAndOthers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<I_InvoiceService, InvoiceService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}