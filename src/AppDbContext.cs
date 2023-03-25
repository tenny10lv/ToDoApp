using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using ToDoAppApi.Modules.AuthModule.Entities;

public class AppDbContext : DbContext
{
	public DbSet<User> Users { get; set; }
	public DbSet<Todo> Todos { get; set; }
	public DbSet<UserToken> UserTokens { get; set; }

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
