using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Cryptography.X509Certificates;
using Grpc.Core;

using EstimateAPI.Authorization;
using EstimateAPI.Configuration;
using EstimateAPI.Filters;
using EstimateAPI.Repository;
using EstimateAPI.Services;
using InternalAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHostedService<CertificateStatusService>();
builder.Services.Configure<ClientDiscoveryOptions>(builder.Configuration.GetSection(ClientDiscoveryOptions.Position));

builder.Services.AddSingleton<IAuthorizationHandler, ApiKeyRequirementHandler>();
builder.Services.AddSingleton<ITelemetryInitializer, FilterHealthchecksTelemetryInitializer>();
builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IEstimateRepository, EstimateRepository>();

builder.Services.AddAuthorization(authConfig =>
{
    authConfig.AddPolicy("apiKey", policyConfig => policyConfig.Requirements.Add(new ApiKeyRequirement()));
});

builder.Services.AddGrpc();
builder.Services.AddGrpcClient<Estimates.EstimatesClient>();
builder.Services.AddGrpcClient<Services.ServicesClient>(o =>
{
    var credentials = CallCredentials.FromInterceptor((context, metadata) =>
    {
        metadata.Add("Authorization", $"token"); // Unused for now
        return Task.CompletedTask;
    });

    var httpHandler = new HttpClientHandler();
    httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    o.Address = new Uri("https://services.api:443");
    o.ChannelOptionsActions.Add(o => o.HttpHandler = httpHandler);
    o.CallOptionsActions.Add(o => o.CallOptions.WithCredentials(credentials));
});

builder.Services.Configure<ListenOptions>(options =>
{
    options.UseHttps(new X509Certificate2(Path.Combine("/certs/tls.crt"), Path.Combine("/certs/tls.key")));
});

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

app.UseAuthorization();

app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api"),
    builder => builder.UseHttpLogging());

app.MapControllers();
app.MapHealthChecks("/healthz");

app.Logger.LogInformation("[EstimateAPI] Finished middleware configuration.. starting the service.");

app.Run();
