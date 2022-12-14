using Microsoft.EntityFrameworkCore;
using ServicesAPI.Data;
using ServicesAPI.Registry;
using ServicesAPI.Repository;
using ServicesAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ServiceContext>();

builder.Services.AddTransient<IServiceRegistry, ServiceRegistry>();
builder.Services.AddTransient<IInternalServices, InternalServices>();
builder.Services.AddTransient<IServiceRepository, ServiceRepository>();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseExceptionHandler(new ExceptionHandlerOptions() { AllowStatusCode404Response = true, ExceptionHandlingPath = "/error" });

app.UseHttpLogging();

app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation("[ServicesAPI] Finished middleware configuration.. starting the service.");

app.Run();
