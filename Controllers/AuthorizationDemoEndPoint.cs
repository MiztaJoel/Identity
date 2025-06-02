using Microsoft.AspNetCore.Authorization;

namespace Identity.Controllers
{
	public static class AuthorizationDemoEndPoint
	{
		public static IEndpointRouteBuilder MapAuthorizationDemoEndPoint(this IEndpointRouteBuilder app)
		{
			app.MapGet("/AdminOnly", AdminOnly);

			app.MapGet("/AdminOrTeacher", [Authorize(Roles = "Admin,Teacher")] () =>
			{
				return "Admin Or Teacher";
			});

			app.MapGet("/LibraryMemberOnly", [Authorize(Policy = "HasLibraryID")] () =>
			{
				return "Library Member Only";
			});

			app.MapGet("/ApplyForMAternityLeave", [Authorize(Roles = "Teacher",Policy = "FemalesOnly")] () =>
			{
				return "Applied for maternity leave.";
			});

			app.MapGet("/Under18sAndFemale",
				[Authorize(Policy = "Under10")]
				[Authorize(Policy ="FemalesOnly")]() =>
			{
				return "Under 18 and female";
			});

			return app;
		}
		[Authorize(Roles ="Admin")]
		private static string AdminOnly()
		{
			return "Admin Only";
		}
	}
}
