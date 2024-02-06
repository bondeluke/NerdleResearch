namespace Solver;

public class Suggestion(string guess, int count)
{
    public string Guess { get; } = guess;
    public int Count { get; } = count;
}