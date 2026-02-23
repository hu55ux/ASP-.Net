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

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Invoice Management System",
                    Description = "Proper management of invoice and customer data.",
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
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using Bearer scheme.\r\n\r\n Example: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id="Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            }
                });
            });
        return services;
    }

    public static IServiceCollection AddInvoiceManagmentDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        services.AddDbContext<InvoiceManagmentDbContext>(
            options => options.UseSqlServer(connectionString)
        );
        return services;
    }

    public static IServiceCollection AddIdentityAndDb(
         this IServiceCollection services,
         IConfiguration configuration)
    {
        services
            .Configure<JwtConfig>(configuration.GetSection(JwtConfig.SectionName));

        services.AddIdentity<AppUser, IdentityRole>(
            options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            }

        )
    .AddEntityFrameworkStores<InvoiceManagmentDbContext>()
    .AddDefaultTokenProviders();
        return services;
    }


    public static IServiceCollection AddAuthenticationAndAuthorization(
         this IServiceCollection services,
         IConfiguration configuration)
    {
        var jwtConfig = new JwtConfig();
        configuration.GetSection(JwtConfig.SectionName).Bind(jwtConfig);


        services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            )
            .AddJwtBearer(
                options =>
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
                }
            );

        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddFluentValidation(
         this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation();
        services
            .AddFluentValidationClientsideAdapters();
        services
            .AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }
    public static IServiceCollection AddAutoMapperAndOthers(
         this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services
            .AddScoped<I_InvoiceService, InvoiceService>();
        services
            .AddScoped<ICustomerService, CustomerService>();
        services
            .AddScoped<IAuthService, AuthService>();

        return services;
    }



}
