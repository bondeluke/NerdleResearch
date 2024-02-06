using Solver;
namespace NerdleResearch;

internal class SolverWrapper(NerdleSolver solver)
{
    private readonly List<List<GuessInfo>> results = [];

    public Suggestion SuggestFirstGuess()
    {
        return solver.SuggestGuess(results);
    }

    public Suggestion SuggestGuess(string previousGuess, string previousResult)
    {
        results.Add(Enumerable.Range(0, 8)
            .Select(i => new GuessInfo(i, previousGuess[i], Convert(previousResult[i])))
            .ToList());

        return solver.SuggestGuess(results);
    }

    private static GuessResult Convert(char result)
    {
        return result switch
        {
            'C' => GuessResult.Correct,
            'P' => GuessResult.Present,
            'A' => GuessResult.Absent,
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }
}