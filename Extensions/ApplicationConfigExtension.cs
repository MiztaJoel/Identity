using Identity.Model;
using Microsoft.EntityFrameworkCore;

namespace Identity.Extensions
{
	public static class ApplicationConfigExtension
	{
		public static WebApplication ConfigureCORS(this WebApplication app, IConfiguration config)
		{
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseCors();
			}
			return app;

		}

		
			public static IServiceCollection AddAppConfig(this IServiceCollection services, IConfiguration config)
			{
			services.Configure<AppSettings>(config.GetSection("AppSettings"));
			return services;
			}
		
	}
}
