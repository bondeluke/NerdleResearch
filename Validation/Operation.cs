namespace Validation
{
    public class Operation(char value) : Symbol()
    {
        public char value = value;

        public override string ToString()
        {
            return value.ToString();
        }
    }
}