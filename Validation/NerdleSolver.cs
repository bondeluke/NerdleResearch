namespace Validation
{
    public enum OccurenceType
    {
        Unknown,
        Exactly,
        AtLeast,
        Dead,
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

    public class NerdleSolver(IList<string> possibleAnswers)
    {
        private List<string> possibilities =
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

        private readonly List<CharacterInformation> characterInformation = "0123456789+-*/"
            .Select(c => new CharacterInformation(c))
            .Append(new CharacterInformation('=', 0, OccurenceType.Exactly))
            .ToList();

        public void DigestGuess(string guess, string result)
        {
            var alive = new List<char>();
            var dead = new List<char>();

            for (var i = 0; i < result.Length; i++)
            {
                var guessedCharacter = guess[i];

                switch (result[i])
                {
                    case 'B':
                        // Black - this character is nowhere in the space of possibilities
                        possibilities = possibilities.Select(p => p.Replace(guessedCharacter.ToString(), "")).ToList();
                        dead.Add(guessedCharacter);
                        break;
                    case 'G':
                        // Green - update possibilities[i] to be the one possibility
                        possibilities[i] = $"{guessedCharacter}";
                        alive.Add(guessedCharacter);
                        break;
                    case 'P':
                        // Purple - remove character at index
                        possibilities[i] = possibilities[i].Replace(guessedCharacter.ToString(), "");
                        alive.Add(guessedCharacter);
                        break;
                    default:
                        throw new ArgumentException($"Unrecognized character '{result[i]}'");
                }
            }

            var aliveCounts = alive.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

            foreach (var aliveChar in aliveCounts)
            {
                var info = characterInformation.Single(ci => ci.Character == aliveChar.Key);
                info.Count = Math.Max(info.Count, aliveChar.Value);
                info.Type = dead.Any(c => c == info.Character) || info.Type == OccurenceType.Exactly
                    ? OccurenceType.Exactly
                    : OccurenceType.AtLeast;
            }

            foreach (var deadChar in dead)
            {
                var info = characterInformation.Single(ci => ci.Character == deadChar);
                info.Count = 0;
                info.Type = OccurenceType.Dead;
            }

            possibleAnswers = UpdatePossibleAnswer(possibleAnswers);

            Console.WriteLine($"My next guess is {possibleAnswers.First()}");
        }

        private IList<string> UpdatePossibleAnswer(IList<string> answers)
        {
            return answers
                .Where(answer => characterInformation
                    .Where(c => c.Type is OccurenceType.AtLeast or OccurenceType.Exactly)
                    .Select(c => c.Character)
                    .All(answer.Contains))
                .Where(answer => answer
                    .Select((_, index) => index)
                    .All(index => possibilities[index].Contains(answer[index]))
                ).ToList();
        }
    }
}
