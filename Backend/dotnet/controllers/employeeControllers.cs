using MongoDB.Driver;
using dotnet.models;
using dotnet.services;
using Microsoft.AspNetCore.Mvc;
namespace dotnet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class employeeControllers : ControllerBase
    {
        private readonly employeeServices _emp;
        public employeeControllers(employeeServices emp)
        {
            _emp = emp;
        }
        [HttpGet]
        public async Task<ActionResult<List<employeeModel>>> GetAll()
        {
            var employees = await _emp.GetAll();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<employeeModel>> Get(string id)
        {
            var emp = await _emp.Get(id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Post(employeeModel emp)
        {
            await _emp.create(emp);
            return Ok("success");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, employeeModel emp)
        {
            var existing = await _emp.Get(id);
            if (existing == null) return NotFound();

            emp.IdNo = id;
            await _emp.update(id, emp);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _emp.Get(id);
            if (existing == null) return NotFound();
            await _emp.Delete(id);
            return NoContent();
        }
    }
}