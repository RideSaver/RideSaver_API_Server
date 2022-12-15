using Microsoft.AspNetCore.HttpOverrides;
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

builder.Services.AddDbContext<ServiceContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ServicesDB");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


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
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseExceptionHandler(new ExceptionHandlerOptions() { AllowStatusCode404Response = true, ExceptionHandlingPath = "/error" });

app.UseHttpLogging();
app.UseAuthorization();
app.MapControllers();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.Logger.LogInformation("[ServicesAPI] Finished middleware configuration.. starting the service.");

app.Run();
