using DeviceCoordinatesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceCoordinatesApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Device> GPS_Device { get; set; }
    public DbSet<User> GPS_MapUser { get; set; }
    public DbSet<TrackingEvents> GPS_TrackingEvents { get; set; }
}
