using Newtonsoft.Json;

namespace DataLibrary
{
    internal class FloatValues
    {
        public float[] values;

        public FloatValues(int lengthOfArray)
        {
            values = new float[lengthOfArray];
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
