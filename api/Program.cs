using api.Controllers;
using api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
                       ?? throw new InvalidOperationException("DB_CONNECTION не задано");

builder.Services.AddDbContext<RailwayContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,            
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null         
        );
    });

    options.EnableSensitiveDataLogging();
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    c.RoutePrefix = "swagger";
});

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");