using Microsoft.EntityFrameworkCore;
using WeddingApi.Data;
using WeddingApi.Repositories;
using WeddingApi.Repositories.Interfaces;
using WeddingApi.Services;
using WeddingApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQLite to use a writable directory in production
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// In production (Railway), use /tmp for database path
if (!builder.Environment.IsDevelopment())
{
    connectionString = "Data Source=/tmp/wedding.db";
}

// Add DbContext
builder.Services.AddDbContext<WeddingDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IWeddingGuestsService, WeddingGuestsService>();

builder.Services.AddScoped<IWeddingGuestsRepository, WeddingGuestsRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5175",
                "http://127.0.0.1:5175",
                "http://localhost:5174",
                "http://127.0.0.1:5174",
                "https://wedding-ui-five.vercel.app",
                "https://wedding-api-production-26a1.up.railway.app")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply migrations and ensure database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var context = services.GetRequiredService<WeddingDbContext>();
        
        logger.LogInformation("Ensuring database is deleted and recreated...");
        
        // Delete and recreate the database to ensure clean state
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        logger.LogInformation("Database created successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while creating the database.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

app.UseCors("AllowReact");
app.UseAuthorization();

// Add a root endpoint for health checks
app.MapGet("/", () => Results.Ok(new { 
    status = "healthy", 
    message = "Wedding API is running",
    endpoints = new[] { "/WeddingGuests" }
}));

app.MapControllers();
app.Run();