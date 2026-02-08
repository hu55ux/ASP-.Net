using ASP_.Net_07_HW.Models;
using ASP_.Net_07_HW.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;

    public TaskItemsController(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        var tasks = await _taskItemService.GetAllAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var task = await _taskItemService.GetByIdAsync(id);
        if (task == null)
            return NotFound($"Task Item with ID:{id} not found!");
        return Ok(task);
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetByProjectId(int projectId)
    {
        var tasks = await _taskItemService.GetByProjectIdAsync(projectId);

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create([FromBody] TaskItem taskItem)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var task = await _taskItemService.CreateAsync(taskItem);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> Update(int id, [FromBody] TaskItem taskItem)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedTask = await _taskItemService.UpdateAsync(id, taskItem);
        if (updatedTask == null)
            return NotFound($"Task with ID:{id} not found!");

        return Ok(updatedTask);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deletedTask = await _taskItemService.DeleteAsync(id);
        if (!deletedTask)
            return NotFound($"Task with ID:{id} not found!");

        return NoContent();
    }
}