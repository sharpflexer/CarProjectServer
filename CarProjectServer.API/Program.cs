using AutoMapper;
using CarProjectServer.API.Controllers;
using CarProjectServer.API.Middleware;
using CarProjectServer.API.Options;
using CarProjectServer.API.Profiles;
using CarProjectServer.BL.Profiles;
using CarProjectServer.BL.Services.Implementations;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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
                          policy.WithOrigins(corsOrigin)
                          .WithMethods("GET", "POST", "PUT", "DELETE")
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
});

builder.Services.AddControllers();

builder.Services.Configure<GoogleOptions>(
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
builder.Services.AddScoped<ITechnicalWorksService, TechnicalWorksService>();

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
app.UseCors(clientOrigin);
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LogMiddleware>();
app.UseMiddleware<TechnicalWorksMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();
 
app.MapControllers();

app.Use(async (context, next) =>
{
    NotificationTimer.StartTimer();

    await next.Invoke();
});

app.Run();

