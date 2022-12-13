using EstimateAPI.Configuration;
using EstimateAPI.Repository;
using InternalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ClientDiscoveryOptions>(builder.Configuration.GetSection(ClientDiscoveryOptions.Position));

builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IEstimateRepository, EstimateRepository>();

builder.Services.AddGrpc();
builder.Services.AddGrpcClient<Services.ServicesClient>(o =>
{
    o.Address = new Uri("http://services-api:80");
});

builder.Services.AddGrpcClient<Estimates.EstimatesClient>("UberClient", o =>
{
    o.Address = new Uri("http://uber-client:80");
});

builder.Services.AddGrpcClient<Estimates.EstimatesClient>("LyftClient", o =>
{
    o.Address = new Uri("http://lyft-client:80");
});
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

app.Logger.LogInformation("[EstimateAPI] Finished middleware configuration.. starting the service.");

app.Run();
