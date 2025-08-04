using DeviceCoordinatesApi.Data;
using DeviceCoordinatesApi.Models;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load file .env
Env.Load();
// Gán Connection String từ biến môi trường
var dbHost = Env.GetString("DB_HOST");
var dbName = Env.GetString("DB_NAME");
var dbUser = Env.GetString("DB_USER");
var dbPass = Env.GetString("DB_PASS");
Console.WriteLine(dbHost);
builder.Configuration["ConnectionStrings:DefaultConnection"] =
    $"Server={dbHost};Database={dbName};User Id={dbUser};Password={dbPass};TrustServerCertificate=True;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
var app = builder.Build();
app.UseCors("AllowAll");
// ===========================
// GET /api/devices
// ===========================
app.MapGet("/api/devices", async (AppDbContext db) =>
{
    var devices = await db.GPS_Device
        .Select(d => new
        {
            d.DeviceID,
            d.Name
        })
        .ToListAsync();

    return Results.Ok(devices);
});

// ===========================
// GET /api/location?deviceId=1&date=2025-07-20
// ===========================
app.MapGet("/api/location", async (string deviceId, string date, AppDbContext db) =>
{
    if (!DateTime.TryParse(date, out var parsedDate))
        return Results.BadRequest("Invalid date format. Use yyyy-MM-dd.");

    var events = await db.GPS_TrackingEvents
        .Where(e => e.Device.DeviceID == deviceId && e.RecordDate.Date == parsedDate.Date)
        .OrderBy(e => e.RecordDate)
        .Select(e => new TrackingEventDto
        {
            OId = e.OId ?? Guid.NewGuid(),
            Latitude = e.Latitude,
            Longitude = e.Longitude,
            Title = e.Title,
            DeviceID = e.DeviceID,
            RecordDate = e.RecordDate,
            Type = e.Type,
            UserName = e.UserName,
            LinkInfo = e.LinkInfo
        })
        .ToListAsync();

    return Results.Ok(events);
});
app.Run();
