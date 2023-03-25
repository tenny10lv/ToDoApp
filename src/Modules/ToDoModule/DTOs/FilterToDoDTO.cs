using System;
namespace ToDoAppApi.Modules.ToDoModule.DTOs
{
	public class FilterToDoDTO
	{
		public int PageIndex { get; set; } = 1;
		public int Take { get; set; } = 10;


		public bool? isOverDue { get; set; }
		public bool? isDone { get; set; }
		public Guid? AssigneeId { get; set; }

		internal int Skip
		{
			get
			{
				return (PageIndex - 1) * Take;
			}
		}
	}
}

