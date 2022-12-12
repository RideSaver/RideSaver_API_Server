using InternalAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServicesAPI.Data;
using ServicesAPI.Registry;
using ServicesAPI.Repository;
using ServicesAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ServiceContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ServicesDB"), x =>
        {
            x.UseNetTopologySuite();
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

      .AddJwtBearer("APIGatewayAuthentication", cfg =>
      {
            cfg.RequireHttpsMetadata = true;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "AuthService",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero

            };
       });


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
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
    app.UseForwardedHeaders();
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseExceptionHandler("/Error");
}

/*using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ServiceContext>();
    dataContext.Database.Migrate();
}*/

app.UseHttpLogging();
app.UseForwardedHeaders();

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation("[ServicesAPI] Finished middleware configuration.. starting the service.");

app.Run();
