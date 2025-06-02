using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Identity.Extensions
{
	public static class SwaggerExtension
	{
		public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "My API",
					Version = "v1",
					Description = "An API to perform Employee operators",

				});
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name="Authorization",
					Type = SecuritySchemeType.Http,
					Scheme="Bearer",
					In=ParameterLocation.Header,
					Description = "Fill in the Jwt",
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id="Bearer",
							}
						},
						new List<string>()
					}

				});
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});


			return services;
		}

		public static WebApplication ConfigureSwaggerExplorer(this WebApplication app)
		{
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
				});
			}
			return app;
		
		}
	}
	
}
