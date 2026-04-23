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

// логин/пароль на свагер
var swaggerUser = Environment.GetEnvironmentVariable("SWAGGER_USER");
var swaggerPass = Environment.GetEnvironmentVariable("SWAGGER_PASS");

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;

    if (path.StartsWith("/swagger"))
    {
        string authHeader = context.Request.Headers["Authorization"];

        if (authHeader != null && authHeader.StartsWith("Basic "))
        {
            var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            var decoded = System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(encodedUsernamePassword)
            );

            var parts = decoded.Split(':');

            var username = parts[0];
            var password = parts[1];

            if (username == swaggerUser && password == swaggerPass)
            {
                await next();
                return;
            }
        }

        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Swagger\"";
        context.Response.StatusCode = 401;
        return;
    }

    await next();
});



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