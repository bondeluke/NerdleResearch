
using Validation;

var answers = Generation.patterns
    .SelectMany(Stuff.Enumerate)
    .Where(o => !o.Contains("/0"))
    .Where(NerdleValidator.IsValidEquation)
    .ToList();

Console.WriteLine(answers.Count);

