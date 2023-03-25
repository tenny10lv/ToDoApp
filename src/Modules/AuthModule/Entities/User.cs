using Microsoft.AspNetCore.Identity;
using ToDoAppApi.Modules.AuthModule.Dtos;

public class User : IdentityUser<Guid>
{
	public string FullName { get; set; }
	public int Age { get; set; }
	public string Role { get; set; }
	public string Address { get; set; }
	public HashSet<Todo> Todos { get; set; }
}
