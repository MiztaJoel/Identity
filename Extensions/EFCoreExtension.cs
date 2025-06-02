using Identity.Model;
using Microsoft.EntityFrameworkCore;

namespace Identity.Extensions
{
	public static class EFCoreExtension
	{
		public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("apicon")));
			return services;
		}
	}
}
