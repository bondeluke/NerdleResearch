
using Solver;
using Validation;

Console.WriteLine("Generating possible answers...");
var answers = Generation.possibleSolutions
    .Where(NerdleValidator.IsValidEquation)
    .ToList();

while (true)
{
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

    Console.WriteLine("Press 'c' to play again...");
    var key = Console.ReadKey();
    if (key.KeyChar != 'c')
        break;
    Console.WriteLine();
}
