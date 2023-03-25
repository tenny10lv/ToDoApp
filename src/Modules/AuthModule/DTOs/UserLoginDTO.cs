using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoAppApi.Modules.AuthModule.Dtos
{
	public record UserLoginDTO
	{
		[Required]
		public string Login { get; init; }

		[Required]
		public string Password { get; init; }
	}

}

