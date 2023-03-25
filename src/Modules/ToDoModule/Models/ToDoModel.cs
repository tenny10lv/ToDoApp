using System;
namespace ToDoAppApi.Modules.ToDoModule.Models
{
	public class ToDoModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsDone { get; set; } = false;
		public DateTime? FinishedDate { get; set; }
		public DateTime DueDate { get; set; }
		public Guid AssignedToId { get; set; }
	}
}

