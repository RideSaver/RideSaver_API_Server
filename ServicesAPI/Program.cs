using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.Data;
using ServicesAPI.Registry;
using ServicesAPI.Repository;
using ServicesAPI.Services;
using System.Security.Cryptography.X509Certificates;

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


builder.Services.AddGrpc();
builder.Services.AddTransient<IServiceRegistry, ServiceRegistry>();
builder.Services.AddTransient<IServiceRepository, ServiceRepository>();


builder.Services.Configure<ListenOptions>(options =>
{
    options.UseHttps(new X509Certificate2(Path.Combine("/certs/tls.crt"), Path.Combine("/certs/tls.key")));
});

IServiceCollection serviceCollection = builder.Services.AddEndpointsApiExplorer();

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
app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapGrpcService<InternalServices>(); });
app.UseHttpLogging();
app.MapControllers();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.Logger.LogInformation("[ServicesAPI] Finished middleware configuration.. starting the service.");

app.Run();
