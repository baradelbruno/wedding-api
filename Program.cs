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

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var usePostgres = false;

// In production (Railway), use PostgreSQL if DATABASE_URL is provided
if (!builder.Environment.IsDevelopment())
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    
    if (!string.IsNullOrEmpty(databaseUrl))
    {
        // Parse Railway's DATABASE_URL format: postgres://user:password@host:port/dbname
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        
        connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
        usePostgres = true;
    }
    else
    {
        // Fallback to SQLite if no DATABASE_URL
        connectionString = "Data Source=/tmp/wedding.db";
    }
}

// Add DbContext with appropriate provider
builder.Services.AddDbContext<WeddingDbContext>(options =>
{
    if (usePostgres)
    {
        options.UseNpgsql(connectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});

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
        
        logger.LogInformation("Applying database migrations...");
        context.Database.Migrate();
        logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
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