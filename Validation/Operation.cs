namespace Validation
{
    public class Operation(char value) : Symbol()
    {
        public char Value = value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}