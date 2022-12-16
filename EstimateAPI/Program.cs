using EstimateAPI.Configuration;
using EstimateAPI.Repository;
using InternalAPI;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ClientDiscoveryOptions>(builder.Configuration.GetSection(ClientDiscoveryOptions.Position));

builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IEstimateRepository, EstimateRepository>();

builder.Services.AddGrpc();

IHttpClientBuilder httpClientBuilder = builder.Services.AddGrpcClient<Services.ServicesClient>(o =>
{
    o.Address = new Uri("https://services-api.api:443");
});

builder.Services.Configure<ListenOptions>(options =>
{
    options.UseHttps(new X509Certificate2(Path.Combine("/certs/tls.crt"), Path.Combine("/certs/tls.key")));
});

builder.Services.AddEndpointsApiExplorer();

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

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseExceptionHandler(new ExceptionHandlerOptions() { AllowStatusCode404Response = true, ExceptionHandlingPath = "/error" });

app.UseHttpLogging();
app.MapControllers();

app.Logger.LogInformation("[EstimateAPI] Finished middleware configuration.. starting the service.");

app.Run();
