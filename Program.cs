using AuthApp.Data;
using AuthApp.Middleware;
using AuthApp.Options;
using AuthApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.SectionName));
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));
builder.Services.AddControllers();
builder.Services.AddScoped<ISystemCLK, SystemCLK>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthentication("Cookies").AddCookie("Cookies", options =>
{
    options.LoginPath = "/api/auth/signin";
    options.Cookie.HttpOnly = true;
});
builder.Services.AddCors(options => { 
    options.AddPolicy("AllowFrontend", policy => 
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();