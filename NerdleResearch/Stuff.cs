public static class Stuff
{
    public static List<String> CrossMultiply(List<String> one, List<String> two)
    {
        return one.SelectMany(o => two.Select(t => o + t)).ToList();
    }

    public static List<String> Convert(char value)
    {
        return value switch
        {
            '1' => Generation.oneDigitNumbers.Select(it => it.ToString()).ToList(),
            '2' => Generation.twoDigitNumbers.Select(it => it.ToString()).ToList(),
            '3' => Generation.threeDigitNumbers.Select(it => it.ToString()).ToList(),
            '#' => Generation.operations.Select(it => it.ToString()).ToList(),
            _ => throw new ArgumentException(),
        };
    }
}