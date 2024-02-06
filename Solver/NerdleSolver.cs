namespace Solver;

public class NerdleSolver(IList<string> answers)
{
    private const int NerdleWidth = 8;
    private readonly Random random = new(DateTime.Now.Millisecond);

    public Suggestion SuggestGuess(List<List<GuessInfo>> guessResults)
    {
        if (guessResults.Count == 0)
        {
            var goodFirstGuesses = answers
                .Where(a => a.IndexOf('=') == 5 && a.Distinct().Count() == 8)
                .ToList();

            return new Suggestion(goodFirstGuesses[random.Next(goodFirstGuesses.Count)], answers.Count);
        }

        var information = "0123456789+-*/"
            .Select(c => new CharacterInfo(c))
            .Append(new CharacterInfo('=', 1, OccurenceType.Exactly))
            .ToList();

        List<string> possibilities =
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

        foreach (var row in guessResults)
        {
            foreach (var characterGuesses in row.GroupBy(c => c.Character))
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
        }

        answers = UpdateAnswers(information, possibilities);

        var unknownCharacters = information
            .Where(c => c.Type == OccurenceType.Unknown)
            .Select(c => c.Character);

        var bestGuesses = answers
            .GroupBy(a => a.Distinct().Intersect(unknownCharacters).Count())
            .OrderByDescending(g => g.Key)
            .First()
            .ToList();

        //Console.WriteLine($"There are {answers.Count} possible answers, but only {bestGuesses.Count} best guesses!");

        return new Suggestion(bestGuesses[random.Next(bestGuesses.Count)], answers.Count);
    }

    private static IEnumerable<int> FindIndicesToCrossOut(IList<GuessInfo> correct, IList<GuessInfo> present, IList<GuessInfo> absent)
    {
        // Some absent characters and no present characters means we can just cross out everything that isn't correct.
        if (absent.Count != 0 && present.Count == 0)
            return Enumerable.Range(0, NerdleWidth)
                .Where(i => correct.All(c => c.Index != i));

        return present.Concat(absent).Select(c => c.Index);
    }

    private List<string> UpdateAnswers(IReadOnlyList<CharacterInfo> information, IReadOnlyList<string> possibilities)
    {
        return answers
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
}