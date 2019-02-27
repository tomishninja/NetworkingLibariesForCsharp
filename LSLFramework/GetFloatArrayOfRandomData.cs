using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSLFramework
{
    /// <summary>
    /// Demo class for how the GetFloatData should work
    /// </summary>
    public class GetFloatArrayOfRandomData : IGetFloatArrayData
    {
        public readonly int ArrayLength = 0;

        public readonly int minRandNum = 0;

        public readonly int maxRandNum = 255;

        private Random random = null;

        public GetFloatArrayOfRandomData(int arrayLength)
        {
            random = new Random();
            this.ArrayLength = arrayLength;
        }

        public GetFloatArrayOfRandomData(int arrayLength, int minRandNum, int maxRandNum)
        {
            if (maxRandNum >= 0 && maxRandNum > minRandNum)
            {
                random = new Random();
                this.ArrayLength = arrayLength;
                this.minRandNum = minRandNum;
                this.maxRandNum = maxRandNum;
            }
        }

        public float[] GetFloatArrayData()
        {
            float[] data = new float[ArrayLength];

            for (int k = 0; k < data.Length; k++)
                data[k] = random.Next(minRandNum, maxRandNum);

            return data;
        }
    }
}
