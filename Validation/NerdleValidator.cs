namespace Validation
{
    public class NerdleValidator
    {
        public static bool IsValidEquation(string input)
        {
            if (input.Length != 8)
                throw new ArgumentException($"{input} does not have exactly 8 characters");

            if (!input.Contains('='))
                throw new ArgumentException($"{input} does not contain a '=' character");

            var parts = input.Split("=");

            var left = Evaluate(parts[0]);
            var right = int.Parse(parts[1]);

            return float.IsInteger(left) && (int)left == right;
        }

        private static float Evaluate(string expression)
        {
            var symbols = ExpressionParser.Parse(expression);

            while (true)
            {
                if (symbols.Count == 1)
                {
                    return ((Number)symbols.Single()).Value;
                }

                var index = FindIndexToEvaluate(symbols);

                var result = Evaluate((Number)symbols[index - 1], (Operation)symbols[index], (Number)symbols[index + 1]);

                symbols = symbols
                    .Take(index - 1)
                    .Append(new Number(result))
                    .Concat(symbols.Skip(index + 2))
                    .ToList();
            }
        }

        private static float Evaluate(Number left, Operation middle, Number right)
        {
            return middle.Value switch
            {
                '+' => left.Value + right.Value,
                '-' => left.Value - right.Value,
                '*' => left.Value * right.Value,
                '/' => left.Value / right.Value,
                _ => throw new ArgumentException($"Invalid operation '{middle.Value}'"),
            };
        }

        private static int FindIndexToEvaluate(IList<Symbol> symbols)
        {
            char[] priorityOperations = ['*', '/'];

            for (var index = 1; index < symbols.Count; index += 2)
            {
                if (priorityOperations.Any(p => p == ((Operation)symbols[index]).Value))
                {
                    return index;
                }
            }

            return 1;
        }
    }
}