using ASP_.NET_InvoiceManagment.Database;
using ASP_.NET_InvoiceManagment.Services;
using ASP_.NET_InvoiceManagment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<InvoiceManagmentDbContext>(
    options => options.UseSqlServer(connectionString)
);
builder.Services.AddScoped<I_InvoiceService, InvoiceService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
