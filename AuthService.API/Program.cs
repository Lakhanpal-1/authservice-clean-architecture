using AuthService.API.Middleware;
using AuthService.Application;
using AuthService.Application.Interfaces.Security;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Persistence.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// Register application layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// -------------------- BUILD APP --------------------

var app = builder.Build();

// -------------------- SEED ADMIN USER --------------------

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    await AdminSeeder.SeedAsync(db, hasher);
}

// -------------------- MIDDLEWARE --------------------

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"))
   .ExcludeFromDescription();

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();




//Add - Migration InitAuthDb - Project AuthService.Infrastructure - StartupProject AuthService.API
//    Update-Database -StartupProject AuthService.API
