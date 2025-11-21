using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RiseApi.Data;
using RiseApi.Helpers.Swagger;
using RiseApi.Services;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var baseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = $"User Id={dbUser};Password={dbPassword};{baseConnection}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString));

var key = builder.Configuration["Jwt:Key"] ?? throw new Exception("JWT Key missing.");
var issuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddScoped<JwtService>();
builder.Services.AddHttpContextAccessor();

builder.Configuration.AddJsonFile(
    "appsettings.Development.json",
    optional: true,
    reloadOnChange: true
);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
