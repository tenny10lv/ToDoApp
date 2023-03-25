using System.ComponentModel.DataAnnotations;
using ToDoAppApi.Utils.Annotations;

namespace ToDoAppApi.Modules.ToDoModule.DTOs
{
	public class CreateOrEditTodoDTO
	{
		internal Guid Id { get; set; }

		[Required]
		public string Title { get; init; }

		[Required]
		public string Description { get; init; }

		[Required]
		[DateGreaterThanCurrentTime]
		public DateTime DueDate { get; init; }

		public Guid? AssignedToId { get; set; }
	}
}
