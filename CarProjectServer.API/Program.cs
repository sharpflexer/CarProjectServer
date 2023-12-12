using AutoMapper;
using CarProjectServer.API.Middleware;
using CarProjectServer.API.Models;
using CarProjectServer.API.Profiles;
using CarProjectServer.BL.Options;
using CarProjectServer.BL.Profiles;
using CarProjectServer.BL.Services.Implementations;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using NLog.Web;

var clientOrigin = "clientOrigin";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: clientOrigin,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                          .WithMethods("GET", "POST", "PUT", "DELETE")
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
});

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

builder.Services.AddAuthentication(
x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {

            ValidIssuer = AuthOptions.Issuer,
            ValidAudience = AuthOptions.Audience,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                }

                return Task.CompletedTask;
            }
        };

    });

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("Create", policy =>
    {
        policy.RequireClaim("CanCreate", "True");
    });
    opts.AddPolicy("Read", policy =>
    {
        policy.RequireClaim("CanRead", "True");
    });
    opts.AddPolicy("Update", policy =>
    {
        policy.RequireClaim("CanUpdate", "True");
    });
    opts.AddPolicy("Delete", policy =>
    {
        policy.RequireClaim("CanDelete", "True");
    });
    opts.AddPolicy("Users", policy =>
    {
        policy.RequireClaim("CanManageUsers", "True");
    });
});

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
