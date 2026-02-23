using System.Text;
using ASP_.NET_InvoiceManagementAuth.Config;
using ASP_.NET_InvoiceManagementAuth.Database;
using ASP_.NET_InvoiceManagementAuth.Extensions;
using ASP_.NET_InvoiceManagementAuth.Middleware;
using ASP_.NET_InvoiceManagementAuth.Models;
using ASP_.NET_InvoiceManagementAuth.Services;
using ASP_.NET_InvoiceManagementAuth.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwagger()
    .AddFluentValidation()
    .AddInvoiceManagmentDBContext(builder.Configuration)
    .AddIdentityAndDb(builder.Configuration)
    .AddAuthenticationAndAuthorization(builder.Configuration)
    .AddAutoMapperAndOthers();






var app = builder.Build();

app.UseInvoiceManagementPipeline();



app.Run();
