using Microsoft.EntityFrameworkCore;
using WeddingApi.Data;
using WeddingApi.Repositories;
using WeddingApi.Repositories.Interfaces;
using WeddingApi.Services;
using WeddingApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Prevent circular reference errors when serializing Gift->Purchases->Gift
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database connection - Always use PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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
    }
}

// Add DbContext with PostgreSQL
builder.Services.AddDbContext<WeddingDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IWeddingGuestsService, WeddingGuestsService>();
builder.Services.AddScoped<IGiftService, GiftService>();
builder.Services.AddScoped<IGiftPurchaseService, GiftPurchaseService>();

builder.Services.AddScoped<IGiftRepository, GiftRepository>();
builder.Services.AddScoped<IGiftPurchaseRepository, GiftPurchaseRepository>();
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
                "https://wedding-api-production-26a1.up.railway.app",
                "https://casamentolarissaebruno.com",
                "https://www.casamentolarissaebruno.com")
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

// Enable static files for serving uploaded images
app.UseStaticFiles();

app.UseCors("AllowReact");
app.UseAuthorization();

// Add a root endpoint for health checks
app.MapGet("/", () => Results.Ok(new { 
    status = "healthy", 
    message = "Wedding API is running",
    endpoints = new[] { "/WeddingGuests", "/Gifts" }
}));

app.MapControllers();
app.Run();