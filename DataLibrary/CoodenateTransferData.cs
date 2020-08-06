namespace DataLibrary
{
    public class CoodenateTransferData
    {
        #region constants
        /// <summary>
        /// the amount of float values within the values array are dedicated
        /// the the float data type
        /// </summary>
        public const int SizeOfBedPlacementData = 3;

        /// <summary>
        /// prefix for unity's JSON parser
        /// </summary>
        public const string JSONPreamble = "{\"Items\":[";

        /// <summary>
        /// postfix for unity's JSON parser
        /// </summary>
        public const string JSONPostamble = "]}";

        /// <summary>
        /// The invalid number for this class. 
        /// If you see a number or id in the class then 
        /// it proberbly hasn't been set. Shouldn't be used on
        /// location based vector coordenates. Designed to 
        /// help with id's
        /// </summary>
        public const float DefaultInvalidNumber = -1f;
        #endregion

        #region veribleDataToTransfer
        /// <summary>
        /// An array of bytes containing metadata for this application
        /// </summary>
        public int[] Meta;

        /// <summary>
        /// Some enums to guide the user for getting information out of the 
        /// metadata IDs
        /// </summary>
        public enum MetaIds
        {
            amountOfMarkers = 0,
            amountOfNeedles = 1,
            sizeOfAMarkerWithinFloatArray = 2,
            sizeOfNeedleWithinFloatArray = 3
        }

        /// <summary>
        /// various float values for many different thing depending on there placement
        /// in the array. 
        /// </summary>
        public float[] values;

        /// <summary>
        /// lables that values may refer to for idification in application
        /// </summary>
        public string[] labels;
        #endregion

        #region PrivateData
        /// <summary>
        /// The current index of the non fild regions of the string arrays.
        /// Starts at zero.
        /// </summary>
        private int indexOfStringArray = -1;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amountOfMarkers"></param>
        /// <param name="amountOfNeedles"></param>
        /// <param name="sizeOfMarkerWithinFloatArray"></param>
        /// <param name="sizeOfNeedleWithinFloatArray"></param>
        public CoodenateTransferData(
            byte amountOfMarkers = 0, byte amountOfNeedles = 0,
            byte sizeOfMarkerWithinFloatArray = 4, byte sizeOfNeedleWithinFloatArray = 7)
        {
            // create the meta array
            Meta = new int[]{
                amountOfMarkers,
                amountOfNeedles,
                sizeOfMarkerWithinFloatArray,
                sizeOfNeedleWithinFloatArray
            };

            // The main position data array plus other data
            values = new float[SizeOfBedPlacementData + 
                (amountOfMarkers * sizeOfMarkerWithinFloatArray) + 
                (amountOfNeedles * sizeOfNeedleWithinFloatArray)];

            // create labels for each of the markers between these points
            labels = new string[amountOfMarkers + amountOfNeedles];

            // set all the values to a number that is more invaid then 0 as this is a useful number
            for(int index = 0; index < values.Length; index++)
            {
                values[index] = DefaultInvalidNumber;
            }
        }

        /// <summary>
        /// sets the bed vector as the user wishes it.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void SetBedVector(float x, float y, float z)
        {
            values[0] = x;
            values[1] = y;
            values[2] = z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float[] GetBedVector()
        {
            return new float[] {values[0], values[1], values[2]};
        }

        /// <summary>
        /// on sets the information regarding the markers. This method assumes the marker
        /// size is set to 4. (O)
        /// </summary>
        /// <param name="MarkerIndex">
        /// The marker your trying to set data for.
        /// </param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void SetMarkerData(int MarkerIndex, string labelInfo, float x, float y, float z)
        {
            // if the values are incorrect stop as this will cause problems with the logic
            if (Meta[(int)MetaIds.amountOfMarkers] == 0 || MarkerIndex < 0 || MarkerIndex > Meta[(int)MetaIds.amountOfMarkers]) return;

            // the index 
            int index = SizeOfBedPlacementData + (Meta[(int)MetaIds.sizeOfAMarkerWithinFloatArray] * MarkerIndex);

            // Set the string data for this item and recive the id for it.
            int id = SetStringInfo(labelInfo, (int)System.Math.Round(values[index]));

            // Check the id so its not invalid if it is don't change this object
            // TODO honestly this should proberbly though an exception 
            if (id == DefaultInvalidNumber) return;

            // set the id
            values[index++] = id;
            // set the coords
            values[index++] = x;
            values[index++] = y;
            values[index] = z;
        }

        /// <summary>
        /// Sends back all the marker data in this object (On).
        /// </summary>
        /// <returns>
        /// A float array of varing size containing all the values 
        /// between the end of the bed data and the start of the needle data
        /// That corrisponds with the meta data.
        /// </returns>
        public float[] GetMarkerData()
        {
            // create an array that can hold all of the data
            float[] outputArray = new float[(Meta[(int)MetaIds.amountOfMarkers] * Meta[(int)MetaIds.sizeOfAMarkerWithinFloatArray])];

            // cacluate the off set from were the bed should be
            int endPoint = SizeOfBedPlacementData + outputArray.Length;
            
            // place all the values into the output array
            for(int valueIndex = SizeOfBedPlacementData; valueIndex != endPoint; valueIndex++)
            {
                outputArray[valueIndex - SizeOfBedPlacementData] = values[valueIndex];
            }

            // and then send it back
            return outputArray;
        }

        public float[] SetNeedleInfo(int NeedleIndex, string labelInfo,
            float ax, float ay, float az, float bx, float by, float bz)
        {
            if (Meta[(int)MetaIds.amountOfMarkers] != 0 && NeedleIndex >= 0 && NeedleIndex < Meta[(int)MetaIds.amountOfNeedles])
            {
                int index = SizeOfBedPlacementData
                    + (NeedleIndex * Meta[(int)MetaIds.sizeOfNeedleWithinFloatArray])
                    + (Meta[(int)MetaIds.amountOfMarkers] * Meta[(int)MetaIds.sizeOfAMarkerWithinFloatArray]);

                // Set the string data for this item and recive the id for it.
                int id = SetStringInfo(labelInfo, (int)System.Math.Round(values[index]));

                // Check the id so its not invalid if it is don't change this object
                // TODO honestly this should proberbly though an exception 
                if (id == DefaultInvalidNumber) return null;

                // set the id
                values[index++] = id;
                // set the coords on both ends of the needle
                values[index++] = ax;
                values[index++] = ay;
                values[index++] = az;
                values[index++] = bx;
                values[index++] = by;
                values[index] = bz;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float[] GetNeedleInfo()
        {
            // the starting index needs to start after the main bed and the marker data
            int index = SizeOfBedPlacementData + (Meta[(int)MetaIds.amountOfMarkers] * Meta[(int)MetaIds.sizeOfAMarkerWithinFloatArray]);

            // the output array
            float[] outputArray = new float[Meta[(int)MetaIds.amountOfNeedles] * Meta[(int)MetaIds.sizeOfNeedleWithinFloatArray]];
            
            // index for the array above should start at zero
            int counter = 0;

            // loop though the array and retrive all of the indexed information regarding this
            while (index < values.Length)
            {
                outputArray[counter++] = values[index++];
            }

            return outputArray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info">
        /// The string you want to place in the array
        /// </param>
        /// <returns>
        /// If the method managed to add the information this method returns 
        /// the index were it placed this information. If it did not manage
        /// to save this information it will return -1
        /// </returns>
        public int SetStringInfo(string info, int index = -1)
        {
            if (index > 0 && index < labels.Length)
            {
                labels[index] = info;
                return index;
            }
            else if (index == DefaultInvalidNumber && indexOfStringArray < labels.Length)
            {
                labels[++indexOfStringArray] = info;
                return indexOfStringArray;
            }
            else
            {
                // if this isnot valid return -1
                return -1;
            }
        }

        /// <summary>
        /// returns a this object as a json script
        /// </summary>
        /// <returns>
        /// A string reprension of this object formated as a json string
        /// </returns>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// returns a this object as a json script formated for unity 
        /// </summary>
        /// <returns>
        /// A string reprension of this object formated as a json string
        /// </returns>
        public string ToUnityJSON()
        {
            return JSONPreamble + ToString() + JSONPostamble;
        }
    }
}
