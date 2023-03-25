using System;
using System.ComponentModel.DataAnnotations;
using ToDoAppApi.Utils.Annotations;

namespace ToDoAppApi.Modules.ToDoModule.Models
{
	public class CreateOrEditToDoModel
	{
		public string Title { get; init; }

		public string Description { get; init; }

		public DateTime DueDate { get; init; }

		public Guid AssignedToId { get; set; }
	}
}

