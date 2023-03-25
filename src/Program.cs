using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using ToDoAppApi.Modules.AuthModule.MapperConfigurations;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var accessTokenSecret = builder.Configuration["Jwt:AccessTokenSecret"];
var isProduction = builder.Environment.IsProduction();

builder.Services.Configure<JsonOptions>(options =>
{
	options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	options.SerializerOptions.Converters.Add(new UserConverter());
});

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition(
		"Bearer",
		new OpenApiSecurityScheme
		{
			Name = "Authorization",
			Type = SecuritySchemeType.Http,
			In = ParameterLocation.Header,
			Scheme = "Bearer",
			BearerFormat = "JWT",
			Description =
				"JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
		}
	);

	options.AddSecurityRequirement(
		new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				new string[] { }
			}
		}
	);
});

// Database Configuration
builder.Services.AddDbContext<AppDbContext>(
	optionsBuilder =>
		optionsBuilder
			.UseSqlServer(builder.Configuration["DatabaseConfiguration:DefaultConnection"])
			.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
			.EnableDetailedErrors()
			.ConfigureWarnings(b => b.Log(ConnectionOpened, CommandExecuted, ConnectionClosed))
);

// Other services injector configuration
builder.Services.AddSingleton<TokenGeneratorService>();
builder.Services.AddSingleton<TokenValidatorService>();

//Mapper configuration
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Filter Configuration
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// Authentication & authorization Configuration
builder.Services
	.AddIdentityCore<User>(options =>
	{
		options.User.RequireUniqueEmail = true;
		options.Password.RequireDigit = isProduction;
		options.Password.RequireLowercase = isProduction;
		options.Password.RequireNonAlphanumeric = isProduction;
		options.Password.RequireUppercase = isProduction;
		if (isProduction)
		{
			options.Password.RequiredLength = 8;
			options.Password.RequiredUniqueChars = 3;
		}
	})
	.AddEntityFrameworkStores<AppDbContext>();

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessTokenSecret)),
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			ValidateIssuerSigningKey = true,
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.Zero
		};
		options.SaveToken = true;
	});

// configure authorize profile
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("", policy => policy.RequireAuthenticatedUser());
});


builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
