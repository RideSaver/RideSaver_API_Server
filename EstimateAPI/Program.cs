using EstimateAPI.Repository;
using EstimateAPI.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InternalAPI;
using Microsoft.AspNetCore.HttpOverrides;

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
    o.Address = new Uri("https://services-api:80");
});

builder.Services.AddGrpcClient<Estimates.EstimatesClient>("UberClient", o =>
{
    o.Address = new Uri("https://uber-client:80");
});

builder.Services.AddGrpcClient<Estimates.EstimatesClient>("LyftClient", o =>
{
    o.Address = new Uri("https://lyft-client:80");
});


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
