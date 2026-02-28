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

// Add DbContext
builder.Services.AddDbContext<WeddingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IWeddingGuestsService, WeddingGuestsService>();

builder.Services.AddScoped<IWeddingGuestsRepository, WeddingGuestsRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5175",
                "http://127.0.0.1:5175",
                "http://localhost:5174",
                "http://127.0.0.1:5174")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

app.UseCors("AllowReact");
app.UseAuthorization();
app.MapControllers();
app.Run();