namespace Solver;

public class NerdleSolver(IList<string> answers)
{
    private const int NerdleWidth = 8;

    private readonly List<string> possibilities =
    [
        "0123456789",
        "0123456789+-*/",
        "0123456789+-*/",
        "0123456789+-*/",
        "0123456789+-*/=",
        "0123456789=",
        "0123456789=",
        "0123456789"
    ];

    private readonly List<CharacterInfo> information = "0123456789+-*/"
        .Select(c => new CharacterInfo(c))
        .Append(new CharacterInfo('=', 1, OccurenceType.Exactly))
        .ToList();

    private readonly Random random = new(DateTime.Now.Millisecond);
    private string? previousGuess;

    public SuggestionResult SuggestFirstGuess()
    {
        var goodGuesses = answers
            .Where(a => a.IndexOf('=') == 5 && a.Distinct().Count() == 8)
            .ToList();

        previousGuess = goodGuesses[random.Next(goodGuesses.Count)];

        return new SuggestionResult(previousGuess, answers.Count);
    }

    public SuggestionResult SuggestNextGuess(string previousGuessResult)
    {
        if (previousGuess is null)
        {
            throw new Exception("Execute method 'SuggestFirstGuess()' first!");
        }

        var guessInfo = Enumerable.Range(0, NerdleWidth)
            .Select(i => new GuessInfo(previousGuess[i], i, Convert(previousGuessResult[i])))
            .GroupBy(c => c.Character);

        foreach (var characterGuesses in guessInfo)
        {
            var correct = characterGuesses.Where(g => g.Result == GuessResult.Correct).ToList();
            var present = characterGuesses.Where(g => g.Result == GuessResult.Present).ToList();
            var absent = characterGuesses.Where(g => g.Result == GuessResult.Absent).ToList();

            var info = information.First(c => c.Character == characterGuesses.Key);

            info.Count = Math.Max(correct.Count + present.Count, info.Count);
            info.Type = info.Type == OccurenceType.Exactly
                ? OccurenceType.Exactly
                : info.Count != 0
                    ? absent.Count != 0
                        ? OccurenceType.Exactly
                        : OccurenceType.AtLeast
                    : OccurenceType.Absent;

            var character = characterGuesses.Key.ToString();

            foreach (var i in correct.Select(c => c.Index))
            {
                possibilities[i] = character;
            }

            foreach (var i in FindIndicesToCrossOut(correct, present, absent))
            {
                possibilities[i] = possibilities[i].Replace(character, string.Empty);
            }
        }

        UpdateAnswers();

        var unknownCharacters = information
            .Where(c => c.Type == OccurenceType.Unknown)
            .Select(c => c.Character);

        var bestGuesses = answers
            .GroupBy(a => a.Distinct().Intersect(unknownCharacters).Count())
            .OrderByDescending(g => g.Key)
            .First()
            .ToList();

        //Console.WriteLine($"There are {answers.Count} possible answers, but only {bestGuesses.Count} best guesses!");

        previousGuess = bestGuesses[random.Next(bestGuesses.Count)];

        return new SuggestionResult(previousGuess, answers.Count); ;
    }

    private static IEnumerable<int> FindIndicesToCrossOut(IList<GuessInfo> correct, IList<GuessInfo> present, IList<GuessInfo> absent)
    {
        // Some absent characters and no present characters means we can just cross out everything that isn't correct.
        if (absent.Count != 0 && present.Count == 0)
            return Enumerable.Range(0, NerdleWidth)
                .Where(i => correct.All(c => c.Index != i));

        return present.Concat(absent).Select(c => c.Index);
    }

    private void UpdateAnswers()
    {
        answers = answers
            .Where(answer =>
            {
                var counts = answer
                    .GroupBy(c => c)
                    .Select(g => (Character: g.Key, Count: g.Count()));
                var exactly = information
                    .Where(c => c.Type is OccurenceType.Exactly)
                    .Select(c => (c.Character, c.Count));
                var atLeast = information
                    .Where(c => c.Type is OccurenceType.AtLeast)
                    .Select(c => (c.Character, c.Count));

                return exactly.All(e => counts.Any(c => c.Character == e.Character && c.Count == e.Count)) &&
                       atLeast.All(a => counts.Any(c => c.Character == a.Character && c.Count >= a.Count));
            })
            .Where(answer => Enumerable.Range(0, NerdleWidth)
                .All(index => possibilities[index].Contains(answer[index]))
            ).ToList();
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