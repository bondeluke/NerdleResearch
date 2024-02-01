namespace NerdleResearch;

public static class Stuff
{
    public static IEnumerable<string> CrossMultiply(IEnumerable<string> one, IEnumerable<string> two)
    {
        return one.SelectMany(o => two.Select(t => o + t));
    }

    public static IEnumerable<string> Translate(char value)
    {
        return value switch
        {
            '1' => Generation.oneDigitNumbers.Select(it => it.ToString()),
            '2' => Generation.twoDigitNumbers.Select(it => it.ToString()),
            '3' => Generation.threeDigitNumbers.Select(it => it.ToString()),
            '#' => Generation.operations.Select(it => it.ToString()),
            '=' => ["="],
            'Z' => Generation.zeroThruNine.Select(it => it.ToString()),
            _ => throw new ArgumentException(),
        };
    }

    public static IEnumerable<string> Enumerate(string pattern)
    {
        return pattern
            .Aggregate(
                new List<string> { "" },
                (value, character) => CrossMultiply(value, Translate(character)).ToList());
    }
}