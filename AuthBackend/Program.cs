using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AuthBackend.Modal;
using Microsoft.AspNetCore.Identity;
using AuthBackend.Interfaces;
using AuthBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// DbContext - Use your connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity (optional - only if using Identity API)
builder.Services
    .AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>();

// Register Services 
builder.Services.AddScoped<ITaskService, TaskService>();

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PlatOps Auth API",
        Version = "v1",
        Description = "Authentication and transaction API for Angular frontend"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatOps Auth API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// ✅ CORS: Must come BEFORE UseAuthentication and UseAuthorization
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:4200")  // ← Only allow Angular dev server
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseAuthentication();   // Required for JWT
app.UseAuthorization();    // Required for [Authorize] attributes

app.UseRouting();

// Map controllers and Identity API
app.MapControllers();

// Optional: Map Identity API endpoints under /api
app.MapGroup("/api").MapIdentityApi<IdentityUser>().RequireAuthorization();

// Alternative: If you don't want Identity, remove the line above

app.Run();