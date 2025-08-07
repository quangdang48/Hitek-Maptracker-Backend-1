using DeviceCoordinatesApi.Data;
using DeviceCoordinatesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviceCoordinatesApi.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class DeviceController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DeviceController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("devices")]
        public async Task<IActionResult> GetDevices()
        {
            var devices = await _db.GPS_Device
                .Select(d => new { d.DeviceID, d.Name })
                .ToListAsync();

            return Ok(devices);
        }

        [HttpGet("location")]
        public async Task<IActionResult> GetLocation([FromQuery] string deviceId, [FromQuery] string date)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
                return BadRequest("Invalid date format. Use yyyy-MM-dd.");

            var events = await _db.GPS_TrackingEvents
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

            return Ok(events);
        }
    }
}
