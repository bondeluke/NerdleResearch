
using NerdleResearch;
using Validation;

var answers = Generation.patterns
    .SelectMany(Generation.enumerate)
    .Where(NerdleValidator.IsValidEquation)
    .ToList();

Console.WriteLine(answers.Count);
