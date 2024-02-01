namespace Validation
{
    public class NerdleValidator
    {
        public static bool IsValidEquation(string input)
        {
            if (input.Length != 8)
                throw new ArgumentException($"{input} does not have exactly 8 characters");

            if (!input.Contains("="))
                throw new ArgumentException($"{input} does not contain a '=' character");

            var parts = input.Split("=");

            var left = Evaluate(parts[0]);
            var right = int.Parse(parts[1]);

            return left == right;
        }

        public static float Evaluate(string expression)
        {
            var result = EvaluateRecursively(ExpressionParser.Parse(expression));

            return ((Number)result.Single()).value;
        }

        private static List<Symbol> EvaluateRecursively(List<Symbol> expression)
        {
            // Console.WriteLine(string.Join(" ",expression.Select(s => s.ToString())));

            if (expression.Count == 1)
            {
                return expression;
            }

            var index = FindIndexToEvaluate(expression);

            var start = expression.Take(index - 1);
            var end = expression.Skip(index + 2);

            var result = Evaluate((Number)expression[index - 1],
            (Operation)expression[index],
            (Number)expression[index + 1]);

            return EvaluateRecursively(start.Append(new Number(result)).Concat(end).ToList());
        }

        private static float Evaluate(Number left, Operation middle, Number right)
        {
            return middle.value switch
            {
                '+' => left.value + right.value,
                '-' => left.value - right.value,
                '*' => left.value * right.value,
                '/' => left.value / right.value,
                _ => throw new ArgumentException($"Ivalid operation '{middle.value}'"),
            };
        }

        private static int FindIndexToEvaluate(List<Symbol> expression)
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