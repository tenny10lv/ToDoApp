using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoAppApi.Modules.AuthModule.Dtos;
using ToDoAppApi.Modules.AuthModule.Entities;
using ToDoAppApi.Modules.AuthModule.Models;
using ToDoAppApi.Modules.ToDoModule.DTOs;
using ToDoAppApi.Modules.ToDoModule.Models;

namespace ToDoAppApi.Modules.ToDoModule.Controllers;

[Route("todo")]
[ApiController]
public class ToDoController : ControllerBase
{
	private readonly ILogger<ToDoController> _logger;
	private readonly IMapper _mapper;
	private readonly AppDbContext _dbContext;
	private readonly UserManager<User> _userManager;
	private readonly TokenGeneratorService _tokenGeneratorService;

	public ToDoController(
		ILogger<ToDoController> logger,
		AppDbContext dbContext,
		IMapper mapper,
		UserManager<User> userManager,
		TokenGeneratorService tokenGeneratorService

	)
	{
		_logger = logger;
		_dbContext = dbContext;
		_mapper = mapper;
		_userManager = userManager;
		_tokenGeneratorService = tokenGeneratorService;
	}

	[HttpGet]
	[Authorize]
	public IActionResult GetTasks([FromQuery] FilterToDoDTO dto)
	{

		if (dto.AssigneeId is null || dto.AssigneeId == Guid.Empty)
			dto.AssigneeId = new Guid(_userManager.GetUserId(this.User)!);

		var result = _dbContext.Todos.Where(todo =>
			(todo.AssignedToId == dto.AssigneeId) &&
			((!dto.isOverDue.HasValue) || (todo.DueDate < DateTime.Now && !todo.IsDone)) &&
			((!dto.isDone.HasValue) || (todo.IsDone == dto.isDone))
		).Skip(dto.Skip).Take(dto.Take).Select(result => _mapper.Map<ToDoModel>(result)).ToArray();

		return Ok(result);
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> CreateTask(CreateOrEditTodoDTO dto)
	{
		if (dto.AssignedToId is null || dto.AssignedToId == Guid.Empty) {
			dto.AssignedToId = new Guid(_userManager.GetUserId(this.User)!);
		}

		var assignee = await _userManager.FindByIdAsync(dto.AssignedToId.ToString()!);
		if (assignee is null)
			return NotFound("User not found to assign");

		
		var toDo = _mapper.Map<Todo>(dto);

		await _dbContext.Todos.AddAsync(toDo);
		await _dbContext.SaveChangesAsync();

		return Ok(_mapper.Map<CreateOrEditToDoModel>(toDo));
	}

	[HttpPut("{id}")]
	[Authorize]
	public async Task<IActionResult> UpdateTask([FromRoute] Guid id, [FromBody] CreateOrEditTodoDTO dto)
	{
		var task = _dbContext.Todos.FirstOrDefault(
			todo => todo.Id == id
		);

		if (task is null) return NotFound("This task not found");

		_mapper.Map<CreateOrEditTodoDTO, Todo>(dto, task);

		_dbContext.Todos.Update(task);
		await _dbContext.SaveChangesAsync();

		return Ok(_mapper.Map<ToDoModel>(task));

	}

	[HttpDelete("{id}")]
	[Authorize]
	public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
	{
		var task = _dbContext.Todos.FirstOrDefault(
			todo => todo.AssignedToId == new Guid(_userManager.GetUserId(this.User)!) && todo.Id == id
		);

		if (task is null) return NotFound("This task not found");


		_dbContext.Todos.Remove(task);
		await _dbContext.SaveChangesAsync();

		return Ok(task);

	}

	[HttpPatch("finish/{id}")]
	[Authorize]
	public async Task<IActionResult> FinishTask([FromRoute] Guid id)
	{
		var task = _dbContext.Todos.FirstOrDefault(
			todo => todo.AssignedToId == new Guid(_userManager.GetUserId(this.User)!) && todo.Id == id
		);

		if (task is null) return NotFound("This task not found");

		task.IsDone = true;
		task.FinishedDate = DateTime.Now;

		_dbContext.Todos.Update(task);
		await _dbContext.SaveChangesAsync();

		return Ok(task);

	}
}
