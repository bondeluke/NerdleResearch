
using Validation;

var answers = Generation.possibleSolutions
    .Where(NerdleValidator.IsValidEquation)
    .ToList();

Console.WriteLine(answers.Count);
