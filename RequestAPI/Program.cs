using RequestAPI.Repository;
using RequestAPI.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;
using InternalAPI;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ClientDiscoveryOptions>(builder.Configuration.GetSection(ClientDiscoveryOptions.Position));


builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IRequestRepository, RequestRepository>();

builder.Services.AddGrpc();
builder.Services.AddGrpcClient<Services.ServicesClient>(o =>
{
    o.Address = new Uri("https://services.api:80");
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
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseForwardedHeaders();
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpLogging();
app.UseForwardedHeaders();

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation("[RequestAPI] Finished middleware configuration.. starting the service.");

app.Run();
