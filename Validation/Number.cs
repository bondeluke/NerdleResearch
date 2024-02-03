namespace Validation
{
    public class Number(float value) : Symbol()
    {
        public float Value = value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}