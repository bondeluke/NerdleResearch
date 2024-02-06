using Microsoft.OpenApi.Models;
using NerdleSolverApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nerdle Solver API", Description = "Let's solve some Nerdle puzzles", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nerdle Solver API V1");
});

app.MapPost("/suggest-guess", NerdleApi.SuggestGuess);

app.Run();