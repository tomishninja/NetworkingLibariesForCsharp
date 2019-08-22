namespace DataLibrary
{
    public class PrototypeMarkers : JSONObjectWrapper
    {
        public new enum DataValues
        {
            HasChanged = 0,
            MainPosX = 1,
            MainPosY = 2,
            MainPosZ = 3,
            MarkerOneX = 4,
            MarkerOneY = 5,
            MarkerOneZ = 6,
            MarkerTwoX = 7,
            MarkerTwoY = 8,
            MarkerTwoZ = 9,
            MarkerThreeX = 10,
            MarkerThreeY = 11,
            MarkerThreeZ = 12,
            MarkerFourX = 13,
            MarkerFourY = 14,
            MarkerFourZ = 15,
            MarkerFiveX = 16,
            MarkerFiveY = 17,
            MarkerFiveZ = 18,
            MarkerSixX = 19,
            MarkerSixY = 20,
            MarkerSixZ = 21
        }

        public PrototypeMarkers() : base(22)
        { }

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

        public float Get(DataValues item)
        {
            return this.data.values[(int)item];
        }

        public float Get(int index)
        {
            return this.data.values[index];
        }
    }
}
