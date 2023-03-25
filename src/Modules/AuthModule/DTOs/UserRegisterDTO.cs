using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoAppApi.Modules.AuthModule.Dtos
{

	public record UserRegisterDTO
	{
		[Required]
		[EmailAddress]
		public string Email { get; init; }

		[Required]
		[DataType(DataType.Password), MinLength(8)]
		public string Password { get; init; }

		[Required]
		[Compare("Password")]
		public string ConfirmPassword { get; init; }

		[Required]
		public string FullName { get; init; }

		[Required]
		public string UserName { get; init; }
		public int Age { get; init; }

		[Required]
		public string Role { get; init; }

		public string Address { get; init; }
	}
}

