namespace DeviceCoordinatesApi.Models;

public class Device
{
    public string? DeviceID { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<TrackingEvents> TrackingEvents { get; set; } = new List<TrackingEvents>();
}
