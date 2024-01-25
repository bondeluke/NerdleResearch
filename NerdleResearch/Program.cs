
using Validation;

var equations = new List<string>()
{
    "5*8-9=31",
    "24/3-6=2",
    "17+2+3=0"
};

foreach (string equation in equations)
{
    var not = NerdleValidator.IsValidEquation(equation) ? "" : "NOT ";

    Console.WriteLine($"{equation} is {not}a valid equation.");
}
