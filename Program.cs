using LogiTrack.Data;
using LogiTrack.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

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



var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");
// Register controller routes.
app.MapControllers();

app.Run();
