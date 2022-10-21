using RideSaverAPI.Data;
using Microsoft.EntityFrameworkCore;
using RideSaverAPI.APIs;

namespace RideSaverAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddDbContext<RiderDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); // Adds the DbContext to the service container.

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

            HTTPClientInstance.InitializeClient(); // Creates one static instance of HTTP client used for the internal API calls. (opens a TCP port for the Uber/Lyft requests)

            app.Run();
        }
    }
}