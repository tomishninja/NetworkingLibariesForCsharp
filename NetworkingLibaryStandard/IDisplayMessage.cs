using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkingLibaryStandard
{
    /// <summary>
    /// This interface should be added to any class 
    /// working with the network stardard libary that recives data
    /// and may have any status that it needs to record. Several
    /// Classes in this name space will require a class with this 
    /// interface to provide safe access to the UI
    /// </summary>
    public interface IDisplayMessage
    {
        /// <summary>
        /// Displays a messages from the class that hasa
        /// </summary>
        /// <param name="type">
        /// The type of message being sent. examples of these could be Data,
        /// Exceptions or Errors.
        /// </param>
        /// <param name="message">
        /// a string primitive represention of the message that was sent from the origin.
        /// </param>
        void DisplayMessage(MessageHelper.MessageType type, string message);
    }
}
