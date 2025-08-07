using System.ComponentModel.DataAnnotations;

namespace DeviceCoordinatesApi.Models;

public class User
{
    [Key]
    public string? username { get; set; }
    public string PIN { get; set; } = null!;

}