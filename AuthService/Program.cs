using AuthService.Data;
using AuthService.Filters;
using AuthService.Repository;
using AuthService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using InternalAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AuthDB"), x => {
            x.UseNetTopologySuite();
        });
});

builder.Services.AddGrpcClient<Authentication.AuthenticationClient>(o =>
{
    o.Address = new Uri($"https://user.api");
});

builder.Services.AddGrpc();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer("GatewayAuthenticationKey", cfg =>
    {
          cfg.RequireHttpsMetadata = true;
          cfg.SaveToken = true;
          cfg.TokenValidationParameters = new TokenValidationParameters()
          {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
                
          };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
    dataContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
