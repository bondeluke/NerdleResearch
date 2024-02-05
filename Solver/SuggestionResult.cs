namespace Solver;

public class SuggestionResult(string guess, int count)
{
    public string Guess { get; } = guess;
    public int Count { get; } = count;
}