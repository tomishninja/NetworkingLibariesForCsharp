namespace DataLibrary
{
    public class JSONObjectWrapper
    {
        /// <summary>
        /// The amount of values that can be entered
        /// </summary>
        internal readonly int AmountOfData = 4;

        /// <summary>
        /// 
        /// </summary>
        public const string JSONPreamble = "{\"Items\":[";

        /// <summary>
        /// 
        /// </summary>
        public const string JSONPostamble = "]}";

        /// <summary>
        /// Data values for all of there stuff 
        /// </summary>
        public enum DataValues
        {
            HasChanged = 0,
            PosX = 1,
            PosY = 2,
            PosZ = 3
        }

        /// <summary>
        /// the main sorce of all of the data in this object
        /// </summary>
        internal readonly FloatValues data;

        /// <summary>
        /// 
        /// </summary>
        public JSONObjectWrapper()
        {
            data = new FloatValues(AmountOfData);
        }

        internal JSONObjectWrapper(int AmountOfData)
        {
            this.AmountOfData = AmountOfData;
            data = new FloatValues(AmountOfData);
        }

        /// <summary>
        /// Set a object within this objects data storage
        /// </summary>
        /// <param name="item">
        /// The data values that could be entered in to the system
        /// </param>
        /// <param name="value">
        /// The new value for the data value chosen
        /// </param>
        public void Set(DataValues item, float value)
        {
            this.data.values[(int)item] = value;
        }

        /// <summary>
        /// The data stucture held in the object
        /// </summary>
        /// <param name="item">
        /// The data values that could be entered in to the system
        /// </param>
        /// <returns>
        /// The float value held in the datastucture
        /// </returns>
        public float Get(DataValues item)
        {
            return this.data.values[(int)item];
        }

        /// <summary>
        /// Returns a string version of the data object.
        /// </summary>
        /// <returns>
        /// A string primive verison of the data object
        /// </returns>
        public string GetJSON(bool unityWrapper = false)
        {
            if (unityWrapper)
            {
                return JSONPreamble + this.data.ToString() + JSONPostamble;
            }
            else
            {
                return this.data.ToString();
            }
        }
    }
}
