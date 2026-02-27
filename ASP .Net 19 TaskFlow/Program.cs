using ASP_.Net_19_TaskFlow.Extensions;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger()
                .AddFluentValidation()
                .AddTaskFlowDbContext(builder.Configuration)
                .AddIdentityAndDb(builder.Configuration)
                .AddJwtAuthenticationAndAuthorization(builder.Configuration)
                .AddCorsPolicy()
                .AddAutoMapperAndOtherServices();

var app = builder.Build();

app.UseTaskFlowPiplene();

await app.EnsureRolesSeededAsync();

app.Run();