namespace Validation
{
    public class Number(float value) : Symbol()
    {
        public float value = value;

        public override string ToString()
        {
            return value.ToString();
        }
    }
}