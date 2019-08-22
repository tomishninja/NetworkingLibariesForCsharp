namespace MovementTestForUnityDemo
{
    class UnityStreamingDataExample
    {
        public const string Deliminator = "_";

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public bool HasChanged { get; set; }

        
        public UnityStreamingDataExample()
        {
            X = 0;
            Y = 0;
            Z = 0;
            HasChanged = false;
        }

        public override string ToString()
        {
            string value =  HasChanged + Deliminator + ToEurope(X) + Deliminator + ToEurope(Y) + Deliminator + ToEurope(Z);
            HasChanged = false;
            return value;
        }

        private string ToEurope(int value, float decrementAmount = 100)
        {
            float decimalValue = value % decrementAmount;
            float wholeValue;
            if (decrementAmount < value)
            {
                wholeValue = value - decrementAmount;
            }
            else
            {
                wholeValue = 0;
            }

            return wholeValue + "," + decimalValue;
        }
    }
}
