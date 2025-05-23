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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title="My API",
        Version="v1",
        Description = "An API to perform Employee operators",

    });
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


//Services from Identity Core
//builder.Services.AddIdentityApiEndpoints<IdentityUser>()
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase= false;
    options.Password.RequireLowercase= false;
    options.User.RequireUniqueEmail = true;

});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("apicon")));

builder.Services.AddAuthentication(x =>
                    {
                        x.DefaultAuthenticateScheme =
                        x.DefaultChallengeScheme=
                        x.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
                    }).AddJwtBearer(y =>
                    {
                        y.SaveToken = false;
                        y.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,    
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!))
                        };
                    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json","My API");
    });
}
#region Config.CORS
app.UseCors();
#endregion
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app
    .MapGroup("/api")
    .MapIdentityApi<IdentityUser>();

app.MapPost("/api/signup", async (UserManager<AppUser> userManager,
	[FromBody] UserRegistrationModel userRegistrationModel) =>
{
    AppUser user = new AppUser()
    {
        UserName = userRegistrationModel.Email,
        Email = userRegistrationModel.Email,
        FullName = userRegistrationModel.FullName
    };
    var result = await userManager.CreateAsync(user, userRegistrationModel.Password);

    if (result.Succeeded)
        return Results.Ok(result);
    else
		return Results.BadRequest(result);
});

app.MapPost("/api/signin", async (
    UserManager < AppUser > userManager,
	[FromBody] LoginModel loginModel) =>
{
    var user =await userManager.FindByEmailAsync(loginModel.Email);

    if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
    {
        var signInKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            {
                new Claim("UserID",user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(10),
            SigningCredentials = new SigningCredentials(
                signInKey,
                SecurityAlgorithms.HmacSha256Signature

                )
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return Results.Ok(new { token });
    }
    else
        return Results.BadRequest(new { message = "Username or password is incorret." });

});


app.Run();

public class UserRegistrationModel()
{
    public string Email { get; set; }
    public string Password { get; set; } 
    public string FullName { get; set; }
}

public class LoginModel()
{
	public string Email { get; set; }
	public string Password { get; set; }
}
