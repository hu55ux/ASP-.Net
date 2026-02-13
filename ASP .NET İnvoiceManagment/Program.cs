using ASP_.NET_InvoiceManagment.Database;
using ASP_.NET_InvoiceManagment.DTOs.CustomerDTOs;
using ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;
using ASP_.NET_InvoiceManagment.Services;
using ASP_.NET_InvoiceManagment.Services.Interfaces;
using ASP_.NET_InvoiceManagment.Validators;
using ASP_.NET_InvoiceManagment.Validators.CustomerValidators;
using ASP_.NET_InvoiceManagment.Validators.InvoiceValidators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
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
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<InvoiceManagmentDbContext>(
    options => options.UseSqlServer(connectionString)
);
builder.Services.AddScoped<I_InvoiceService, InvoiceService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IValidator<CreateCustomerRequest>, CreateCustomerValidator>();
builder.Services.AddScoped<IValidator<UpdateCustomerRequest>, UpdateCustomerValidator>();
builder.Services.AddScoped<IValidator<CreateInvoiceRequest>, CreateInvoiceValidator>();
builder.Services.AddScoped<IValidator<UpdateInvoiceRequest>, UpdateInvoiceValidator>();
builder.Services.AddScoped<IValidator<CustomerQueryDTO>, CustomerQueryValidator>();
builder.Services.AddScoped<IValidator<InvoiceQueryDTO>, InvoiceQueryValidator>();

builder.Services.AddFluentValidationAutoValidation();


builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice Management API v1");
            options.RoutePrefix = string.Empty;
            options.DisplayRequestDuration();
            options.EnableFilter();
            options.EnableDeepLinking();
            options.EnableTryItOutByDefault();
        }
        );
}

app.UseAuthorization();

app.MapControllers();

app.Run();
