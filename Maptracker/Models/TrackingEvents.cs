using System.ComponentModel.DataAnnotations;

namespace DeviceCoordinatesApi.Models;

public class TrackingEvents
{
    [Key]
    public Guid? OId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Title { get; set; } = null!;
    public string? DeviceID { get; set; }
    public Device Device { get; set; } = null!;
    public DateTime RecordDate { get; set; } = DateTime.UtcNow;
    public int Type { get; set; }
    public string? UserName { get; set; }
    public string? LinkInfo { get; set; }
}
