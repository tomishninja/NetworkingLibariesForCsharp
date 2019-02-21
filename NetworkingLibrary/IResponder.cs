namespace NetworkingLibrary
{
    /// <summary>
    /// This interface allows classes in the Networking libary to
    /// send various interactions. More still needs to be done with this class
    /// before its intergartion
    /// </summary>
    interface IResponder
    {
        /// <summary>
        /// This method will be called to determine various 
        /// responces that can be called vai the network server.
        /// </summary>
        /// <param name="MessageRecived">
        /// The data recived from the message recived.
        /// </param>
        void Respond(string MessageRecived);
    }
}
