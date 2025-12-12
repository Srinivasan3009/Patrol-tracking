using dotnet.models;
using dotnet.services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace dotnet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _user;
        public UserController(UserServices user)
        {
            _user = user;
        }
        [HttpGet("getuser")]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _user.GetAll();
            return Ok(employees);
        }
        [HttpPut("updateuser")]
        public async Task<IActionResult> Update(UpdateUserModel model)
        {
            await _user.Update(model);
            return Ok(new { message = "User Updated" });
        }
        [HttpDelete("deleteuser/{idNo}")]
        public async Task<IActionResult> Delete(string idNo)
        {
            await _user.delete(idNo);
            return Ok(new {message="User Deleted"});
        }
    }
}