namespace Solver;

public class NerdleSolver(IList<string> answers)
{
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

    public void DigestGuess(string equation, string result)
    {
        var guessInfo = result
            .Select((_, i) => new GuessInfo(equation[i], i, Convert(result[i])))
            .GroupBy(c => c.Character);

        foreach (var guess in guessInfo)
        {
            var character = guess.Key.ToString();
            var info = information.First(c => c.Character == guess.Key);
            var dead = guess.Where(g => g.Result == GuessResult.Dead).ToList();
            var correct = guess.Where(g => g.Result == GuessResult.Correct).ToList();
            var somewhere = guess.Where(g => g.Result == GuessResult.Somewhere).ToList();

            info.Count = Math.Max(correct.Count + somewhere.Count, info.Count);
            info.Type = info.Type == OccurenceType.Exactly
                ? OccurenceType.Exactly
                : info.Count != 0
                    ? dead.Count != 0
                        ? OccurenceType.Exactly
                        : OccurenceType.AtLeast
                    : OccurenceType.Dead;

            foreach (var g in correct)
            {
                possibilities[g.Index] = character;
            }
            foreach (var g in somewhere.Concat(dead))
            {
                possibilities[g.Index] = possibilities[g.Index].Replace(character, "");
            }

            if (somewhere.Count != 0 || dead.Count == 0) continue;

            // Dead characters, and no somewhere means we can cross out a few more.
            var extraIndexesToCrossOut = Enumerable
                .Range(0, possibilities.Count)
                .Where(i => correct.All(c => c.Index != i));

            foreach (var index in extraIndexesToCrossOut)
            {
                possibilities[index] = possibilities[index].Replace(character, "");
            }
        }

        UpdateAnswers();

        Console.WriteLine($"My next guess is {answers.First()}");
    }

    private void UpdateAnswers()
    {
        answers = answers
            .Where(answer => information
                .Where(c => c.Type is OccurenceType.AtLeast or OccurenceType.Exactly)
                .Select(c => c.Character)
                .All(answer.Contains))
            .Where(answer => answer
                .Select((_, index) => index)
                .All(index => possibilities[index].Contains(answer[index]))
            ).ToList();
    }

    private static GuessResult Convert(char result)
    {
        return result switch
        {
            'G' => GuessResult.Correct,
            'P' => GuessResult.Somewhere,
            'B' => GuessResult.Dead,
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }
}