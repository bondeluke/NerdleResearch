using Microsoft.OpenApi.Models;
using NerdleSolverApi;

NerdleApi.SuggestGuess([]); // Bootstrap Answers
var builder = WebApplication.CreateBuilder(args);

const string allowNerdlePolicy = "ALLOW_NERDLE_POLICY";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowNerdlePolicy, policy =>
        policy
        .WithOrigins("https://nerdlegame.com")
        .AllowAnyHeader()
        .AllowAnyMethod());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nerdle Solver API", Description = "Let's solve some Nerdle puzzles", Version = "v1" });
});

var app = builder.Build();

app.UseCors(allowNerdlePolicy);
app.UseStaticFiles(); // UseStaticFiles() must be after UseCors(...)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nerdle Solver API V1");
});

app.MapPost("/suggest-guess", NerdleApi.SuggestGuess);

app.Run();