namespace Solver;

public enum GuessResult
{
    Correct,
    Present,
    Absent
}

public record GuessInfo(int Index, char Character, GuessResult Result)
{
    public override string ToString()
    {
        return $"{Index} | {Character} | {Result}";
    }
}