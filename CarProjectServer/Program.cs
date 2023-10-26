using AutoMapper;
using CarProjectServer.API.Models;
using CarProjectServer.API.Profiles;
using CarProjectServer.BL.Profiles;
using CarProjectServer.BL.Services.Implementations;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
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
    typeof(ApiToBlProfile),
    typeof(BlToApiProfile),
    typeof(BlToDalProfile),
    typeof(DalToBlProfile)
    );
builder.Services.AddScoped<IMapper, Mapper>();

builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<IEmailService, EmailService>();

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
