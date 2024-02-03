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

        public static float Evaluate(string expression)
        {
            var result = Evaluate(ExpressionParser.Parse(expression));

            return ((Number)result.Single()).value;
        }

        private static IEnumerable<Symbol> Evaluate(IList<Symbol> expression)
        {
            while (true)
            {
                if (expression.Count == 1)
                {
                    return expression;
                }

                var index = FindIndexToEvaluate(expression);

                var result = Evaluate((Number)expression[index - 1], (Operation)expression[index], (Number)expression[index + 1]);

                expression = expression
                    .Take(index - 1)
                    .Append(new Number(result))
                    .Concat(expression.Skip(index + 2))
                    .ToList();
            }
        }

        private static float Evaluate(Number left, Operation middle, Number right)
        {
            return middle.value switch
            {
                '+' => left.value + right.value,
                '-' => left.value - right.value,
                '*' => left.value * right.value,
                '/' => left.value / right.value,
                _ => throw new ArgumentException($"Invalid operation '{middle.value}'"),
            };
        }

        private static int FindIndexToEvaluate(IList<Symbol> expression)
        {
            char[] priorityOperations = ['*', '/'];

            for (var index = 1; index < expression.Count; index += 2)
            {
                var operation = ((Operation)expression[index]).value;

                if (priorityOperations.Any(p => p == operation))
                {
                    return index;
                }

            }

            return 1;
        }
    }
}