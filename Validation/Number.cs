namespace Validation
{
    public class Number(int value) : Symbol()
    {
        public int value = value;

        public override string ToString()
        {
            return value.ToString();
        }
    }
}