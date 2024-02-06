
using NerdleResearch;
using Solver;
using Validation;

ConsoleHelper.WriteLine("Generating possible answers...", ConsoleColor.DarkYellow);
var answers = Generation.possibleSolutions
    .Where(NerdleValidator.IsValidEquation)
    .ToList();

while (true)
{
    var solver = new SolverWrapper(new NerdleSolver(answers));
    var proposition = solver.SuggestFirstGuess();

    while (true)
    {
        ConsoleHelper.Write($"Out of {proposition.Count} possible answers, try ");
        ConsoleHelper.WriteLine(proposition.Guess, ConsoleColor.DarkMagenta);
        ConsoleHelper.Write("Result: ");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        var guessResult = Console.ReadLine();
        proposition = solver.SuggestGuess(proposition.Guess, guessResult);
        if (proposition.Count == 1)
        {
            ConsoleHelper.Write("The answer is ");
            ConsoleHelper.WriteLine(proposition.Guess, ConsoleColor.DarkMagenta);
            break;
        }
    }

    ConsoleHelper.WriteLine("Press 'p' to play again, any other key to quit.", ConsoleColor.DarkYellow);
    var key = Console.ReadKey();
    if (key.KeyChar != 'p')
        break;
    Console.WriteLine();
}
