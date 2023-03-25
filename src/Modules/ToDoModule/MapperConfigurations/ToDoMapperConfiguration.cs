using System;
using AutoMapper;
using ToDoAppApi.Modules.ToDoModule.DTOs;
using ToDoAppApi.Modules.ToDoModule.Models;

namespace ToDoAppApi.Modules.ToDoModule.MapperConfigurations
{
	public class ToDoMapperConfiguration : Profile
	{
		public ToDoMapperConfiguration()
		{
			CreateMap<CreateOrEditTodoDTO, Todo>();
			CreateMap<Todo, CreateOrEditToDoModel>();
			CreateMap<Todo, ToDoModel>();
		}
	}
}

