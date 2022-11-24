using Microsoft.EntityFrameworkCore;
using ServicesAPI.Data;
using ServicesAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ServiceContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ServicesDB"), x => {
            x.UseNetTopologySuite();
        });
});

builder.Services.AddTransient<IServiceRepository, ServiceRepository>();

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
