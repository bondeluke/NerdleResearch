
using Solver;
using Validation;

Console.WriteLine("Generating possible answers...");
var answers = Generation.possibleSolutions
    .Where(NerdleValidator.IsValidEquation)
    .ToList();

var solver = new NerdleSolver(answers);

solver.SuggestFirstGuess();

while (true)
{
    Console.WriteLine("What's the result?!");
    var result = Console.ReadLine();
    var solved = solver.DigestGuess(result);
    if (solved)
        break;
}

Console.WriteLine("Press any key to exit...");
Console.ReadLine();
