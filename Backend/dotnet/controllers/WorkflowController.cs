using dotnet.services;
using dotnet.models;
using Microsoft.AspNetCore.Mvc;
namespace dotnet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowController : ControllerBase
    {
        private readonly WorkflowServices _wf;
        public WorkflowController(WorkflowServices wf)
        {
            _wf = wf;
        }
        [HttpPost("addwf")]
        public async Task<IActionResult> Addwf([FromBody] WorkflowModel flow)
        {
            var result = await _wf.AddWf(flow);
            if (result == "Success")
                return Ok(new { message = "Workflow added successfully" });

            return BadRequest(new { message = "Failed to add Workflow " });
        }
        [HttpGet("getwf")]
        public async Task<IActionResult> GetAll()
        {
            var wf = await _wf.GetAll();
            return Ok(wf);
        }
        [HttpPut("updatewf")]
        public async Task<IActionResult> UpdateWf(WorkflowModel model)
        {
            await _wf.UpdateWf(model);
            return Ok(new { message = "Workflow Updated" });
        }
        [HttpDelete("deletewf/{wfid}")]
        public async Task<IActionResult> Delete(string wfid)
        {
            await _wf.DeleteWf(wfid);
            return Ok(new {message="Workflow Deleted"});
        }
    }
}