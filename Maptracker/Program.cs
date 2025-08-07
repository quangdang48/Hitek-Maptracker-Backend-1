using DeviceCoordinatesApi.Data;
using DeviceCoordinatesApi.Models;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;



var builder = WebApplication.CreateBuilder(args);

// Load file .env
Env.Load();
// Gán Connection String từ biến môi trường
var dbHost = Env.GetString("DB_HOST");
var dbName = Env.GetString("DB_NAME");
var dbUser = Env.GetString("DB_USER");
var dbPass = Env.GetString("DB_PASS");
builder.Configuration["ConnectionStrings:DefaultConnection"] =
    $"Server={dbHost};Database={dbName};User Id={dbUser};Password={dbPass};TrustServerCertificate=True;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
// Thêm Authentication
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyAuthCookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
        options.LoginPath = "/api/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        // Exception
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();
// Cache
builder.Services.AddDistributedMemoryCache();
// Session
builder.Services.AddSession();
//Add controller 
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("AllowFrontend");
// ============Authentication===============
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
// ====================Add Controller======================
app.MapControllers();
app.Run();
