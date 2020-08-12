namespace NetworkingLibrary
{
    public interface ITCPResponder
    {
        /// <summary>
        /// This interface allows the user to determine a method that will
        /// read the input from a data string from a TCP connection and read it
        /// as it was on 
        /// </summary>
        /// <param name="data">
        /// The data recived from the last package
        /// </param>
        /// <returns>
        /// The message to send respond with
        /// </returns>
        string Respond(ref string data);
    }
}
