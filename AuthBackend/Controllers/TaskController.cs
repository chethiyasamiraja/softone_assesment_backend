using Application.Wrappers;
using AuthBackend.Interfaces;
using AuthBackend.Modal; 
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;

namespace AuthBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
         
        [HttpGet]
        public async Task<ActionResult<Responses<List<TaskEntity>>>> GetAll()
        {
            var response = await _taskService.GetAllTasksAsync();
            return Ok(response);
        }
         
        [HttpGet("{id}")]
        public async Task<ActionResult<Responses<TaskEntity>>> GetById(int id)
        {
            var response = await _taskService.GetTaskByIdAsync(id);
            return response.Succeeded ? Ok(response) : NotFound(response);
        }
         
        [HttpPost]
        public async Task<ActionResult<Responses<TaskEntity>>> Create([FromBody] TaskEntity task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Responses<TaskEntity>
                {
                    Succeeded = false,
                    Message = "Validation failed.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var response = await _taskService.CreateTaskAsync(task);
            return response.Succeeded ? CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response) : BadRequest(response);
        }
         
        [HttpPut("{id}")]
        public async Task<ActionResult<Responses<TaskEntity>>> Update(int id, [FromBody] TaskEntity task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Responses<TaskEntity>
                {
                    Succeeded = false,
                    Message = "Validation failed.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var response = await _taskService.UpdateTaskAsync(id, task);
            return response.Succeeded ? Ok(response) : NotFound(response);
        }
         
        [HttpDelete("{id}")]
        public async Task<ActionResult<Responses<bool>>> Delete(int id)
        {
            var response = await _taskService.DeleteTaskAsync(id);
            return response.Succeeded ? Ok(response) : NotFound(response);
        }


        [HttpPost("validate")]
        public async Task<ActionResult<LoginResponse>> Validate([FromBody] JsonElement body)
        {
            var username = body.GetProperty("username").GetString();
            var password = body.GetProperty("password").GetString();

            var response = await _taskService.ValidateUserAsync(username, password);

            if (response.IsValid)
            {
                return Ok(response);
            }
            else
            {
                return Unauthorized(response); // Or BadRequest
            }
        }











    }
}