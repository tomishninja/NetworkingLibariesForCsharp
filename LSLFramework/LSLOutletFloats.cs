using LSL;

namespace LSLFramework
{
    public class LSLOutletFloats : AbstractLSLObject
    {
        /// <summary>
        /// The name of the stream
        /// </summary>
        private readonly string streamName = null;

        /// <summary>
        /// The type of data being sent over the stream (can be set to anything inlet just needs to match)
        /// </summary>
        private readonly string type = null;

        /// <summary>
        /// 
        /// </summary>
        private readonly string sourceID = null;

        /// <summary>
        /// 
        /// </summary>
        private readonly int channelCount = 4;

        /// <summary>
        /// 
        /// </summary>
        private readonly int nominalState = 100;

        /// <summary>
        /// 
        /// </summary>
        private readonly IGetFloatArrayData dataGenerator = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataGenerator"></param>
        public LSLOutletFloats(IGetFloatArrayData dataGenerator)
        {
            this.dataGenerator = dataGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamName"></param>
        /// <param name="streamType"></param>
        /// <param name="dataGenerator"></param>
        public LSLOutletFloats(string streamName, string streamType, IGetFloatArrayData dataGenerator)
        {
            this.streamName = streamName;
            type = streamType;
            this.dataGenerator = dataGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamName">
        /// 
        /// </param>
        /// <param name="streamType">
        /// 
        /// </param>
        /// <param name="dataGenerator">
        /// 
        /// </param>
        public LSLOutletFloats(string streamName, string streamType, string sourceID, IGetFloatArrayData dataGenerator)
        {
            this.streamName = streamName;
            type = streamType;
            this.sourceID = sourceID;
            this.dataGenerator = dataGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void RunLSL()
        {
            // set the meta data of the stream
            liblsl.StreamInfo info = new liblsl.StreamInfo(
                this.streamName, this.type, channelCount, nominalState,
                liblsl.channel_format_t.cf_float32, this.sourceID);

            // make the LSL outlet object based on the stream info
            liblsl.StreamOutlet outlet = new liblsl.StreamOutlet(info);

            // this array will hold the data being sent each time
            float[] data = new float[arrayLength];

            // while 
            while (this.keepThreadRunning)
            {
                // get the data
                data = dataGenerator.GetFloatArrayData();

                // send the data out into the stream
                outlet.push_sample(data);

                // wait until the next piece of info is needed
                System.Threading.Thread.Sleep(this.delayInMiliseconds);
            }
        }
    }
}
