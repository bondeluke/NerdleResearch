namespace Solver;

internal enum GuessResult
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