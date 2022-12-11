using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.UseForwardedHeaders();
//.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
