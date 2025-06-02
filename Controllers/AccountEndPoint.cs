using Identity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.Controllers
{
	public static  class AccountEndPoint
	{
		public static IEndpointRouteBuilder MapAccountEndPoints(this IEndpointRouteBuilder app)
		{
			app.MapGet("/UserProfile", GetUserProfile);

			return app;

		}
		[Authorize]
		private static async Task<IResult> GetUserProfile(ClaimsPrincipal user,UserManager<AppUser> userManager)
		{
			string userId =user.Claims.First(x => x.Type == "UserID").Value;
			var useDetails =await userManager.FindByIdAsync(userId);
			return Results.Ok(
			new
			{
				Email = useDetails?.Email,
				FullName = useDetails?.FullName,
			}); 
		}
	}
}
