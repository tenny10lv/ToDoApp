using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoAppApi.Modules.AuthModule.Entities
{
	public record UserToken
	{
		[Key]
		public Guid Id { get; init; }
		public bool isRevoked { get; set; }
		public Guid UserId { get; init; }

		public User User { get; set; }
	}
}

