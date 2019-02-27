using LSL;

namespace LSLFramework
{
    public class LSLInletFloats : AbstractLSLObject
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string prop = null;

        public const string DefaultProp = "type";

        /// <summary>
        /// 
        /// </summary>
        private readonly string value = null;

        public const string DefaultValue = "stuff";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public LSLInletFloats(string prop, string value, int arrayLength)
        {
            this.prop = prop;
            this.value = value;
            this.arrayLength = arrayLength;
        }

        public LSLInletFloats(string prop, string value, int arrayLength, IDisplayMessage messageSystem)
            : base(messageSystem)
        {
            this.prop = prop;
            this.value = value;
            this.arrayLength = arrayLength;
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void RunLSL()
        {
            // wait until an EEG stream shows up
            liblsl.StreamInfo[] results = liblsl.resolve_stream(prop, value);

            // open an inlet and print some interesting info about the stream (meta-data, etc.)
            liblsl.StreamInlet inlet = new liblsl.StreamInlet(results[0]);
            System.Console.Write(inlet.info().as_xml());

            // read samples
            float[] sample = new float[this.arrayLength];
            while (this.keepThreadRunning)
            {
                // get the new sample
                inlet.pull_sample(sample);

                // Turn the sample array into a string so it can be output
                string lslOutput = ToString(sample);
                
                // display the whole line as is
                messageSystem.DisplayMessage(lslOutput);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array">
        /// 
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        private string ToString(float[] array)
        {
            // the output string that this array creates
            string output = "";

            // loop though the array to create the string
            for (int i = 0; i < array.Length; i++)
            {
                output += array[i];

                if(i < array.Length - 1)
                {
                    output += ",";
                }
            }

            return output;
        }
    }
}
