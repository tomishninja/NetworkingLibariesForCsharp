using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingLibrary
{
    /// <summary>
    /// Allows comunication from the network libary to a class
    /// that this is connected to.
    /// </summary>
    public interface IDisplayMessage
    {
        /// <summary>
        /// sends a message to a object from the network class
        /// so long as this object is used as a parameter in the constuctor.
        /// </summary>
        /// <param name="message">
        /// The message text from the network libary
        /// </param>
        void DisplayMessage(string message);
    }
}
