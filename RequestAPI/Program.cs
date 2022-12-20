using InternalAPI;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using RequestAPI.Configuration;
using RequestAPI.Repository;
using System.Security.Cryptography.X509Certificates;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddGrpc();
        builder.Services.Configure<ClientDiscoveryOptions>(builder.Configuration.GetSection(ClientDiscoveryOptions.Position));
        builder.Services.AddSingleton<IClientRepository, ClientRepository>();
        builder.Services.AddTransient<IRequestRepository, RequestRepository>();

        builder.Services.AddGrpcClient<Services.ServicesClient>(o =>
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            o.Address = new Uri("https://services.api:443");
            o.ChannelOptionsActions.Add(o => o.HttpHandler = httpHandler);
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

        app.UseHttpLogging();
        app.MapControllers();

        app.Logger.LogInformation("[RequestAPI] Finished middleware configuration.. starting the service.");

        app.Run();
    }
}
