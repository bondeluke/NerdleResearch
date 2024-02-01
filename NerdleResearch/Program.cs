
using Validation;

var equations = new List<string>()
{
    "5*8-9=31",
    "24/3-6=2",
    "17+2+3=0"
};

Generation.printPatterns();

foreach (string equation in equations)
{
    var not = NerdleValidator.IsValidEquation(equation) ? "" : "NOT ";

    Console.WriteLine($"{equation} is {not}a valid equation.");
}

var options = new List<string>();

foreach (var pattern in Generation.patterns)
{
    var shortTermCandidates = Stuff.Convert(pattern[0]);
    for (int i = 1; i < pattern.Length; i++)
    {
        if (i < pattern.Length - 1)
        {
            shortTermCandidates = Stuff.CrossMultiply(shortTermCandidates, Stuff.Convert('#'));
        }
        else
        {
            shortTermCandidates = shortTermCandidates.Select(x => $"{x}=").ToList();
        }

        shortTermCandidates = Stuff.CrossMultiply(shortTermCandidates, Stuff.Convert(pattern[i]));
    }
    Console.WriteLine(shortTermCandidates.Count);
    options = options.Concat(shortTermCandidates).ToList();
}

options = options.Where(o => !o.Contains("/0")).ToList();


Console.WriteLine(options.Count);

var theAnswer = options.Where(NerdleValidator.IsValidEquation).ToList();

Console.WriteLine(theAnswer.Count);

var theNiceEquations = theAnswer.Where(a => !a.EndsWith("=0")).ToList();

Console.WriteLine(theNiceEquations.Count);
