using AutoMapper;
using CarProjectServer.API.Middleware;
using CarProjectServer.API.Models;
using CarProjectServer.API.Options;
using CarProjectServer.API.Profiles;
using CarProjectServer.BL.Options;
using CarProjectServer.BL.Profiles;
using CarProjectServer.BL.Services.Implementations;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using NLog.Web;

var clientOrigin = "clientOrigin";

var builder = WebApplication.CreateBuilder(args);
var corsOrigin = builder.Configuration
    .GetSection("Cors")
    .GetValue<string>("Origin");

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: clientOrigin,
                      policy =>
                      {
                          policy.WithOrigins()
                          .WithMethods("GET", "POST", "PUT", "DELETE")
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
});

builder.Services.AddControllers();

builder.Services.Configure<CarProjectServer.API.Options.GoogleOptions>(
    builder.Configuration.GetSection("GoogleOptions"));

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(connection);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddAutoMapper(
    typeof(ApiCarProfile),
    typeof(ApiUserProfile),
    typeof(BlCarProfile),
    typeof(BlUserProfile)
    );

builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddHttpClient("Google");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
    )
    .AddJwtBearer(options => CarJwtBearerOptions.GetInstance(options));

builder.Services.AddAuthorization(options => CarAuthorizationOptions.GetInstance(options));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LogMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(clientOrigin);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
