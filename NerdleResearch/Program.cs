
using NerdleResearch;
using Solver;
using Validation;

ConsoleHelper.WriteLine("Generating possible answers...", ConsoleColor.DarkYellow);
var answers = Generation.possibleSolutions
    .Where(NerdleValidator.IsValidEquation)
    .ToList();

while (true)
{
    var solver = new NerdleSolver(answers);
    var proposition = solver.SuggestFirstGuess();
    ConsoleHelper.Write("Your first guess should be ");
    ConsoleHelper.WriteLine(proposition.Guess, ConsoleColor.DarkMagenta);

    while (true)
    {
        ConsoleHelper.Write("Result: ");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        var guessResult = Console.ReadLine();
        proposition = solver.SuggestNextGuess(guessResult);
        if (proposition.Count == 1)
        {
            ConsoleHelper.Write("The answer is ");
            ConsoleHelper.WriteLine(proposition.Guess, ConsoleColor.DarkMagenta);
            break;
        }
        ConsoleHelper.Write($"Out of {proposition.Count} possible answers, try ");
        ConsoleHelper.WriteLine(proposition.Guess, ConsoleColor.DarkMagenta);
    }

    ConsoleHelper.WriteLine("Press 'p' to play again, any other key to quit.", ConsoleColor.DarkYellow);
    var key = Console.ReadKey();
    if (key.KeyChar != 'p')
        break;
    Console.WriteLine();
}
