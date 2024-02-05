
using Solver;
using Validation;

Console.WriteLine("Generating possible answers...");
var answers = Generation.possibleSolutions
    .Where(NerdleValidator.IsValidEquation)
    .ToList();

while (true)
{
    var solver = new NerdleSolver(answers);
    var proposition = solver.SuggestFirstGuess();
    Console.WriteLine($"Your first guess should be: {proposition.Guess}");

    while (true)
    {
        Console.WriteLine("What's the result?!");
        var guessResult = Console.ReadLine();
        proposition = solver.SuggestNextGuess(guessResult);
        if (proposition.Count == 1)
        {
            var fgc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"The answer is {proposition.Guess}!!!");
            Console.ForegroundColor = fgc;
            break;
        }
        Console.WriteLine($"Out of {proposition.Count} possible answers, try {proposition.Guess}");
    }

    Console.WriteLine("Press 'p' to play again...");
    var key = Console.ReadKey();
    if (key.KeyChar != 'p')
        break;
    Console.WriteLine();
}
