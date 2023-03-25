using System;
using AutoMapper;
using ToDoAppApi.Modules.AuthModule.Dtos;

namespace ToDoAppApi.Modules.AuthModule.MapperConfigurations
{
	public class AuthMapperConfiguration : Profile
	{
		public AuthMapperConfiguration()
		{
			CreateMap<UserRegisterDTO, User>();
		}
	}
}

