using AutoMapper;
using CarProjectServer.API.Middleware;
using CarProjectServer.API.Models;
using CarProjectServer.API.Profiles;
using CarProjectServer.BL.Profiles;
using CarProjectServer.BL.Services.Implementations;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

builder.Services.AddIdentity<UserViewModel, RoleViewModel>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddUserManager<UserManager<UserViewModel>>()
    .AddSignInManager<SignInManager<UserViewModel>>();

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

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LogMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
