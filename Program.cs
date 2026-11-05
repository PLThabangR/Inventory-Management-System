using LogiTrack.Data;
using LogiTrack.Models;
using Microsoft.EntityFrameworkCore;
//Mapster
using LogiTrack.Mappings;
//IDentity 
using Microsoft.AspNetCore.Identity;
using LogiTrack.Models.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using LogiTrack.Service;
using LogiTrack.Services;

var builder = WebApplication.CreateBuilder(args);
// Register Mapster mappings.
MappingConfig.RegisterMappings();

// Register controllers.
builder.Services.AddControllers();

// Register Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//DB context 
builder.Services.AddDbContext<LogiTrackContext>(options =>
    options.UseSqlite("Data Source=logitrack.db"));

//

//Asp .Net Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configure password requirements
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;

    options.User.RequireUniqueEmail = true;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan =TimeSpan.FromMinutes(15);

}).AddEntityFrameworkStores<LogiTrackContext>().AddDefaultTokenProviders();
    
//JWT Configuration
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is missing");

 
 // Get the secret key from configuration and create a symmetric security key
 var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret));

 builder.Services.AddAuthentication(options =>
 {
     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
 }).AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration["Jwt:Issuer"],
         ValidAudience = builder.Configuration["Jwt:Audience"],
         //this key is always unique and should be kept secret. It is used to sign the JWT token.
         IssuerSigningKey = key,
         ClockSkew = TimeSpan.Zero
     };
 });

//Authorization 
builder.Services.AddAuthorization();

//JWT Service 
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

//Enable Memory Caching 
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapGet("/", () => "Health check endpoint. The API is running.");

// Authentication must come before authorization.
app.UseAuthentication();
app.UseAuthorization();
// Register controller routes.
app.MapControllers();

app.Run();
