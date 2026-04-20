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
// מחיקת כל הנסיונות הקודמים - כתיבה ישירה של המחרוזת שלך
var connectionString = "Server=bjlbe5xvchnwpmdsdfgi-mysql.services.clever-cloud.com;Port=3306;Database=bjlbe5xvchnwpmdsdfgi;Uid=uc0apnhnkcrdhsng;Pwd=LBSJLeuBmWCuB1aAfSHt;";

builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
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
        policy.WithOrigins("https://todolistreact-master-vnp7.onrender.com") // הכתובת של ה-React
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

//Server=bjlbe5xvchnwpmdsdfgi-mysql.services.clever-cloud.com;Port=3306;Database=bjlbe5xvchnwpmdsdfgi;User=uc0apnhnkcrdhsng;Password=LBSJLeuBmWCuB1aAfSHt;