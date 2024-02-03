namespace Validation
{
    internal class ExpressionParser
    {
        public static List<Symbol> Parse(string expression)
        {
            var symbols = new List<Symbol>();
            var number = string.Empty;

            foreach (var c in expression)
            {
                var operation = IsOperation(c);

                if (operation is null)
                {
                    number += c;
                }
                else
                {
                    // When we find an operation, add the preceding number, then the operation
                    symbols.Add(new Number(int.Parse(number)));
                    symbols.Add(new Operation((char)operation));
                    number = string.Empty;
                }
            }

            // Finally, append the last number
            return [.. symbols, new Number(int.Parse(number))];
        }

        private static char? IsOperation(char c)
        {
            return c switch
            {
                '+' => c,
                '-' => c,
                '*' => c,
                '/' => c,
                _ => null,
            };
        }
    }
}