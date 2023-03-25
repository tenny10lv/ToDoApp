using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoAppApi.Modules.AuthModule.Dtos;
using ToDoAppApi.Modules.AuthModule.Entities;
using ToDoAppApi.Modules.AuthModule.Models;

namespace ToDoAppApi.Modules.AuthModule.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly ILogger<AuthController> _logger;
	private readonly IMapper _mapper;
	private readonly AppDbContext _dbContext;
	private readonly UserManager<User> _userManager;
	private readonly TokenGeneratorService _tokenGeneratorService;

	public AuthController(
		ILogger<AuthController> logger,
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

	[HttpPost("sign-in")]
	public async Task<IActionResult> SignIn(UserLoginDTO dto)
	{
		var isUsingEmailAddress = EmailAddressValidator.TryValidate(dto.Login, out var _);

		_logger.LogInformation(isUsingEmailAddress.ToString());
		_logger.LogInformation(dto.ToString());

		var user = isUsingEmailAddress
					? await _userManager.FindByEmailAsync(dto.Login)
					: await _userManager.FindByNameAsync(dto.Login);
		if (user is null) return NotFound("User with this email address not found.");

		_logger.LogInformation(user.ToString());
		if (user is null) return NotFound("User with this email address not found.");

		var result = await _userManager.CheckPasswordAsync(user, dto.Password);
		if (!result) return BadRequest("Incorrect password.");


		var accessToken = _tokenGeneratorService.GenerateAccessToken(user);

		await _dbContext.SaveChangesAsync();
		return Ok(new TokenResponseModel()
		{
			Token = accessToken
		});
	}

	[HttpPost("sign-up")]
	public async Task<IActionResult> SignUp(UserRegisterDTO dto)
	{
		if (_userManager.Users.Any(u => u.Email == dto.Email))
			return Conflict("Invalid `email`: A user with this email address already exists.");
		if (_userManager.Users.Any(u => u.UserName == dto.UserName))
			return Conflict("Invalid `username`: A user with this username already exists.");

		var user = _mapper.Map<User>(dto);
		var result = await _userManager.CreateAsync(user, dto.Password);
		if (!result.Succeeded) return BadRequest(result.Errors);

		return Ok(result);
	}
}
