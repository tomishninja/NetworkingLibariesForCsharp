using Newtonsoft.Json;

namespace DataLibrary
{
    public class AutomatedJSONTestWrapperVOne
    {
        public float TableHorizontal;
        public float TableVertical;

        public Marker[] Markers;

        /// <summary>
        /// prefix for unity's JSON parser
        /// </summary>
        public const string JSONPreamble = "{\"Items\":[";

        /// <summary>
        /// postfix for unity's JSON parser
        /// </summary>
        public const string JSONPostamble = "]}";

        public AutomatedJSONTestWrapperVOne(float TableHorizontal, float TableVertical)
        {
            this.TableHorizontal = TableHorizontal;
            this.TableVertical = TableVertical;

            Markers = new Marker[0];
        }

        public int AppendMarkers(Marker marker)
        {
            int placeOfLastItem = Markers.Length;

            Marker[] temp = new Marker[placeOfLastItem + 1];

            for (int index = 0; index < Markers.Length; index++)
            {
                temp[index] = Markers[index];
            }

            temp[placeOfLastItem] = marker;

            Markers = temp;

            return placeOfLastItem;
        }

        public int AppendMarkers(Marker[] RangeOfMarkers)
        {
            Marker[] temp = new Marker[Markers.Length + RangeOfMarkers.Length];

            int index;
            for (index = 0; index < Markers.Length; index++)
                temp[index] = Markers[index];

            int counter = 0;
            for (; index < temp.Length; index++)
                temp[index] = RangeOfMarkers[counter++];

            Markers = temp;

            return counter;
        }

        public bool RemoveMarkers(Marker markerToBeRemoved)
        {
            if (markerToBeRemoved == null) return false;

            Marker[] temp = new Marker[Markers.Length - 1];

            int tempIndex = 0;
            for (int MainIndex = 0; MainIndex < Markers.Length; MainIndex++)
            {
                if (!Markers[MainIndex].Equals(markerToBeRemoved))
                {
                    temp[tempIndex++] = Markers[MainIndex];

                    // if there was no entry to remove state that and move on forward
                    if (tempIndex >= temp.Length && tempIndex != MainIndex 
                        && !Markers[MainIndex+1].Equals(markerToBeRemoved))
                    {
                        return false;
                    }
                }
            }

            // replace the old array with the new one
            Markers = temp;

            // everything worked so return true;
            return true;
        }

        public string ToJSON()
        {
            return JSONPreamble + JsonConvert.SerializeObject(this) + JSONPostamble;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
