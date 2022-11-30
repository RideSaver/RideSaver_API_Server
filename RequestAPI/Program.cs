using DataAccess.Data;
using RequestAPI.Repository;
using RequestAPI.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ClientDiscoveryOptions>(
    builder.Configuration.GetSection(ClientDiscoveryOptions.Position));

builder.Services.AddDbContext<RSContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("UserDB"), x => x.UseNetTopologySuite());
});

builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IRequestRepository, RequestRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
