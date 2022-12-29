using EstimateAPI.Configuration;
using EstimateAPI.Repository;
using Grpc.Core;
using InternalAPI;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.Configure<ClientDiscoveryOptions>(builder.Configuration.GetSection(ClientDiscoveryOptions.Position));

builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IEstimateRepository, EstimateRepository>();

builder.Services.AddGrpc();
builder.Services.AddGrpcClient<Estimates.EstimatesClient>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(authConfig =>
{
    authConfig.AddPolicy("apiKey", policyBuilder => policyBuilder);
});

builder.Services.AddGrpcClient<Services.ServicesClient>(o =>
{
    var credentials = CallCredentials.FromInterceptor((context, metadata) =>
    {
        metadata.Add("Authorization", $"token"); // Unused for now
        return Task.CompletedTask;
    });

    var httpHandler = new HttpClientHandler();
    httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    o.Address = new Uri("https://services-api.api:443");
    o.ChannelOptionsActions.Add(o => o.HttpHandler = httpHandler);
    o.CallOptionsActions.Add(o => o.CallOptions.WithCredentials(credentials));
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

app.UseAuthorization();


app.UseHttpLogging();
app.MapControllers();
app.MapHealthChecks("/healthz");

app.Logger.LogInformation("[EstimateAPI] Finished middleware configuration.. starting the service.");

app.Run();
