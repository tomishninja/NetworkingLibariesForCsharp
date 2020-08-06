namespace NetworkingLibaryStandard
{
    /// <summary>
    /// By Thomas Clarke
    /// 
    /// This was a class that was developed with the aim of allowing this service
    /// to send huge amounts of data between different devices. This class here gives the
    /// system a method of parsing this information, 
    /// As well as a method of deliminating this information. 
    /// </summary>
    class XMLDeliminator
    {
        /// <summary>
        /// Is there a start module for this data that is in an XML format
        /// </summary>
        /// <param name="data">
        /// The string data that we need to find the index of
        /// </param>
        /// <param name="index">
        /// this 
        /// </param>
        /// <returns>
        /// 1 == yes there is a start modual,
        /// 0 == The characters exist but they are in the wrong order 
        /// -1 == no there is no start modual in this example
        /// </returns>
        public short HasXMLStartModual(string data, out int index)
        {
            index = data.IndexOf('<');
            if (index < 0)
            {
                // if the index dosn't exist then don't check futher
                return -1;
            }
            else
            {
                // if it does then check a bit futher
                return findXMLTagEnding(ref index, data);
            }
        }

        /// <summary>
        /// Is there an end module for this data that is in an XML format
        /// </summary>
        /// <param name="data">
        /// The string data that we need to find the index of
        /// </param>
        /// <param name="start">
        /// A optional parameter that allows the user to choose a start space for the were they expect the end to be placed at. 
        /// </param>
        /// <returns>
        /// 1 == yes there is an end modual,
        /// 0 == The characters exist but they are in the wrong order 
        /// -1 == no there is no start modual in this example
        /// </returns>
        public short HasXMLEndTag(string data, out int index, int start = 0)
        {
            // start looking for the end string after the starting pos
            index = data.IndexOf("</", start);
            if (index < 0)
            {
                return -1;
            }
            else
            {
                // work out were the end tag is for this stucture
                return findXMLTagEnding(ref index, data);
            }
        }

        /// <summary>
        /// just makes sure the XML module has an ending 
        /// </summary>
        /// <param name="start">
        /// A value that this will be input as the start of the end data tag but then it before 
        /// the end of the system it will be updated to reflect the end state
        /// </param>
        /// <param name="data"> 
        /// The data that this system will look thought to determine the result of the this method. 
        /// </param>
        /// <returns></returns>
        private short findXMLTagEnding(ref int start, string data)
        {
            // start looking from were the user wants to start from and if it isn't found the result will be -1
            int indexOfEnd = data.IndexOf('>', start);

            // if index of end isn't before the start or after the end
            // Now this only is just a glorified -1 checker
            if (start < indexOfEnd)
            {
                // set the parameter as the input verible and tell the end user that this is correct
                start = indexOfEnd;
                return 1;
            }

            // set the parameter as the input verible and tell the end user that this is not going to be ok. 
            start = indexOfEnd;
            return 0;
        }
    }
}
