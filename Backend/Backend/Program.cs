using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Backend.Data; // Assuming your ApplicationDbContext is in Backend.Data

// The application builder is declared here once:
var builder = WebApplication.CreateBuilder(args);

// --- JWT Configuration Setup ---
var configuration = builder.Configuration;

// IMPORTANT: Store the key securely. Reading from appsettings.json is done here.
var jwtSecretKey = configuration["Jwt:Key"] ??
                   throw new InvalidOperationException("JWT Secret Key not found in configuration.");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
var issuer = configuration["Jwt:Issuer"];
var audience = configuration["Jwt:Audience"];


// Add services to the container.

// 1. Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// 2. Add Authentication Service
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = key
    };
});

// 3. Add Authorization Service
builder.Services.AddAuthorization();


// Standard setup
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- JWT Middleware Setup (MUST be before app.MapControllers()) ---
app.UseAuthentication(); // Must be before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
