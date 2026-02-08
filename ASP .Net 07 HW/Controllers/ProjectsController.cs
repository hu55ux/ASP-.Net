using ASP_.Net_07_HW.Models;
using ASP_.Net_07_HW.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace ASP_.Net_07_HW.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetAll()
    {
        var projects = await _projectService.GetAllAsync();
        return Ok(projects);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetById(int id)
    {
        var project = await _projectService.GetByIdAsync(id);

        if (project == null)
            return NotFound($"Project with ID:{id} not found!");

        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<Project>> Create([FromBody] Project project)
    {
        if (ModelState.IsValid)
            return BadRequest(ModelState);

        var createdProject = await _projectService.CreateAsync(project);

        return CreatedAtAction("GetById", new { id = createdProject.Id }, createdProject);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Project>> Update(int id, [FromBody] Project project)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updatedProject = await _projectService.UpdateAsync(id, project);
        if (updatedProject == null)
            return NotFound($"Project with ID:{id} not found");

        return Ok(updatedProject);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Project>> Delete(int id)
    {
        var deletedProject = await _projectService.DeleteAsync(id);
        if (!deletedProject)
            return NotFound($"Project with ID:{id} not found!");
        return NoContent();
    }

}
