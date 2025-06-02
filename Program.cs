using Identity.Controllers;
using Identity.Extensions;
using Identity.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Web.Services3.Security.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Services from Identity Core
//builder.Services.AddIdentityApiEndpoints<IdentityUser>()
/* *********This can done when the return type of the service was void
builder.Services.AddIdentityHandlerAndStores();
builder.Services.ConfigureIdentityOptions(); 
*/

//Best approach when the service is return
builder.Services.AddSwaggerExplorer()
                 .InjectDbContext(builder.Configuration)
                 .AddAppConfig(builder.Configuration)
				 .AddIdentityHandlerAndStores()
				 .ConfigureIdentityOptions()
                .AddIdentityAuth(builder.Configuration);
 
//This is added above as AddAppConfig
//builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var app = builder.Build();

app.ConfigureSwaggerExplorer()
    .ConfigureCORS(builder.Configuration)
    .AddIdentityAuthMiddlewares() ;

app.UseHttpsRedirection();

app.MapControllers();

app
    .MapGroup("/api")
    .MapIdentityApi<IdentityUser>();

app.MapGroup("/api")
    .MapIdentityUserEndPoint()
    .MapAccountEndPoints()
    .MapAuthorizationDemoEndPoint();

app.Run();