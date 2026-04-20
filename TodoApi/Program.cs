using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApi.Models;


var builder = WebApplication.CreateBuilder(args);

// 1. הוספת תמיכה ב-Controllers 
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. הגדרת מחרוזת החיבור ל-MySQL
// var connectionString = builder.Configuration.GetConnectionString("ToDoDB");
// if (connectionString != null && connectionString.Contains("name="))
// {
//     connectionString = connectionString.Replace("name=", "Database=");
// }
// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// במקום להשתמש ב-GetConnectionString, נמשוך את הערך ישירות מה-Environment
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ToDoDB");

// אם זה לא נמצא ב-Environment (בזמן הרצה מקומית למשל), ננסה את הדרך הרגילה
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("ToDoDB");
}

builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    if (!string.IsNullOrEmpty(connectionString))
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
});
// 3. הגדרת אימות JWT
var secretKey = builder.Configuration["Jwt:Key"] ?? "default_secret_key_at_least_32_chars";// משיכת המפתח מה-appsettings.json
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero // מבטל דיליי של פקיעת תוקף
    };
});
// 4. הגדרת CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
// 5. סדר ה-Middleware קריטי: קודם אימות ואז הרשאות
app.UseAuthentication(); 
app.UseAuthorization();

// 6. מיפוי ה-Controllers (במקום MapGet הישנים)
app.MapControllers();

app.Run();