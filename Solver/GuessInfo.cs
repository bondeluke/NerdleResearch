namespace Solver;

public enum GuessResult
{
    Correct,
    Present,
    Absent
}

public class GuessInfo(int index, char character, GuessResult result)
{
    public int Index { get; } = index;
    public char Character { get; } = character;
    public GuessResult Result { get; } = result;

    public override string ToString()
    {
        return $"{Index} | {Character} | {Result}";
    }
}