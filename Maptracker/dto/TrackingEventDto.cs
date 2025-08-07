namespace DeviceCoordinatesApi.Models;
public class TrackingEventDto
{
    public Guid OId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Title { get; set; }
    public string? DeviceID { get; set; }
    public DateTime RecordDate { get; set; }
    public int Type { get; set; }
    public string? UserName { get; set; }
    public string? LinkInfo { get; set; }
}