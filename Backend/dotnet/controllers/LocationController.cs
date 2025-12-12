using Microsoft.AspNetCore.Mvc;
using dotnet.services;
using dotnet.models;

namespace dotnet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _service;

        public LocationController(LocationService service)
        {
            _service = service;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("get/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var location = await _service.GetByCodeAsync(code);
            return location == null ? NotFound() : Ok(location);
        }

        [HttpPost("addLoc")]
        public async Task<IActionResult> Create(LocationModel model)
        {
            await _service.AddAsync(model);
            return Ok(new { message = "Location added successfully" });
        }

        [HttpPut("update/{code}")]
        public async Task<IActionResult> Update(string code, LocationModel model)
        {
            var updated = await _service.UpdateAsync(code, model);
            return updated ? Ok(new { message = "Location updated" }) : NotFound();
        }

        [HttpDelete("delete/{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var deleted = await _service.DeleteAsync(code);
            return deleted ? Ok(new { message = "Location deleted" }) : NotFound();
        }
    }
}
