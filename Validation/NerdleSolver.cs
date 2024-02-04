namespace Validation
{
    public enum OccurenceType
    {
        Unknown,
        Exactly,
        AtLeast,
        Dead
    }

    public enum GuessResult
    {
        Correct,
        Somewhere,
        Dead
    }

    internal class GuessInfo(char character, int index, GuessResult result)
    {
        public char Character { get; } = character;
        public int Index { get; } = index;
        public GuessResult Result { get; } = result;

        public override string ToString()
        {
            return $"{Character} at {Index} | {Result}";
        }
    }

    public class CharacterInformation(char character, int count = -1, OccurenceType type = OccurenceType.Unknown)
    {
        public char Character = character;
        public int Count = count;
        public OccurenceType Type = type;

        public override string ToString()
        {
            return Type switch
            {
                OccurenceType.AtLeast => $"{Character} | at least {Count}",
                OccurenceType.Exactly => $"{Character} | exactly {Count}",
                OccurenceType.Unknown => $"{Character} | ?",
                OccurenceType.Dead => $"{Character} | DEAD",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

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

        private readonly List<CharacterInformation> information = "0123456789+-*/"
            .Select(c => new CharacterInformation(c))
            .Append(new CharacterInformation('=', 1, OccurenceType.Exactly))
            .ToList();

        public void DigestGuess(string equation, string result)
        {
            var guessInfo = result
                .Select((_, i) => new GuessInfo(equation[i], i, Convert(result[i])))
                .GroupBy(c => c.Character)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var guess in guessInfo)
            {
                var character = guess.Key.ToString();
                var info = information.First(c => c.Character == guess.Key);
                var dead = guess.Value.Where(g => g.Result == GuessResult.Dead).ToList();
                var correct = guess.Value.Where(g => g.Result == GuessResult.Correct).ToList();
                var somewhere = guess.Value.Where(g => g.Result == GuessResult.Somewhere).ToList();

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
                if (somewhere.Count == 0 && dead.Count != 0)
                {
                    var extraIndexesToCrossOut = Enumerable
                        .Range(0, possibilities.Count)
                        .Where(i => correct.All(c => c.Index != i));

                    foreach (var index in extraIndexesToCrossOut)
                    {
                        possibilities[index] = possibilities[index].Replace(character, "");
                    }
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
}
