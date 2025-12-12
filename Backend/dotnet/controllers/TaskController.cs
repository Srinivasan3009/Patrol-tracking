using MongoDB.Driver;
using dotnet.services;
using dotnet.models;
using dotnet.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
namespace dotnet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskServices _task;
        private readonly IHubContext<TaskNotificationHub> _hubContext;
        public TaskController(TaskServices Task, IHubContext<TaskNotificationHub> hubContext)
        {
            _task = Task;
            _hubContext = hubContext;
        }
        [HttpPost("addtask")]
        public async Task<IActionResult> AddTask([FromBody] TaskModel task)
        {
            var result = await _task.Addtask(task);
            if (result == "Success")
                return Ok(new { message = "Task added successfully" });

            return BadRequest(new { message = "Failed to add Task" });
        }
        [HttpGet("gettask")]
        public async Task<IActionResult> GetAll()
        {
            var Task = await _task.GetAll();
            return Ok(Task);
        }
        [HttpPut("updatetask")]
        public async Task<IActionResult> UpdateTask(TaskModel model)
        {
            await _task.Updatetask(model);
            return Ok(new { message = "Task Updated" });
        }
        [HttpDelete("deletetask/{idNo}")]
        public async Task<IActionResult> Delete(string idNo)
        {
            await _task.Deletetask(idNo);
            return Ok(new { message = "Task Deleted" });
        }
        [HttpPut("assigntask")]
        public async Task<IActionResult> AssignTask([FromBody] AssignTaskRequest request)
        {
            var success = await _task.AssignTaskAsync(request.Taskname, request.Name);
            await _hubContext.Clients.User("inno").SendAsync("ReceiveTaskNotification", request.Taskname);
            if (!success)
                return NotFound(new { message = "Task not found" });
            return Ok(new { message = "Task assigned successfully" });
        }
    }
}