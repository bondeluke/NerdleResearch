using Solver;
using Validation;

namespace NerdleSolverApi
{
    public static class NerdleApi
    {
        private static readonly List<string> Answers = Generation.possibleSolutions
            .Where(NerdleValidator.IsValidEquation)
            .ToList();

        public static SuggestionDto SuggestGuess(NerdleGuessResultDto[] results)
        {
            return new NerdleSolver(Answers)
                .SuggestGuess(results.FromDto())
                .ToDto();
        }

        private static List<List<GuessInfo>> FromDto(this IEnumerable<NerdleGuessResultDto> results)
        {
            return results
                .Select(row =>
                Enumerable.Range(0, 8)
                    .Select(i => new GuessInfo(i, row.Guess[i], row.Result[i].ToGuessResult()))
                    .ToList())
                .ToList();
        }

        private static SuggestionDto ToDto(this Suggestion suggestion)
        {
            return new SuggestionDto(suggestion.Guess, suggestion.Count);
        }

        private static GuessResult ToGuessResult(this char result)
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
}
