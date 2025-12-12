using MongoDB.Driver;
using dotnet.services;
using dotnet.models;
using dotnet.controllers;
using Microsoft.AspNetCore.Mvc;
namespace dotnet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleServices _role;
        public RoleController(RoleServices role)
        {
            _role = role;
        }
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRole([FromBody] RoleModel role)
        {
            var result = await _role.AddRole(role);
            if (result == "Success")
                return Ok(new{message="Role added successfully"});

            return BadRequest(new{message= "Failed to add role"});
        }
        [HttpGet("getrole")]
        public async Task<IActionResult> GetAll()
        {
            var role = await _role.GetAll();
            return Ok(role);
        }
        [HttpPut("updaterole")]
        public async Task<IActionResult> UpdateRole(RoleModel model)
        {
            await _role.UpdateRole(model);
            return Ok(new { message = "Role Updated" });
        }
        [HttpDelete("deleterole/{idNo}")]
        public async Task<IActionResult> Delete(string idNo)
        {
            await _role.DeleteRole(idNo);
            return Ok(new {message="Role Deleted"});
        }
    }
}