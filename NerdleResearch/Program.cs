
using Validation;

var answers = Generation.possibleSolutions
    .Where(NerdleValidator.IsValidEquation)
    .ToList();

var solver = new NerdleSolver(answers);

//while (true)
//{
//    Console.WriteLine("What's your guess and result?!");
//    var guess = Console.ReadLine();
//    solver.DigestGuess(guess[..8], guess[8..16]);
//}

solver.DigestGuess("8-3+7=12", "PBPPGGBB");
solver.DigestGuess("33+47=80", "GBGGGGGB");
solver.DigestGuess("39+47=86", "GGGGGGGG");



