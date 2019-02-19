using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingLibrary
{
    public class NetworkingLibaryCore
    {
        /// <summary>
        /// This is a default port number that this Libary will use
        /// if none other is provided.
        /// </summary>
        public const string DefaultServiceName = "22112";

        /// <summary>
        /// A string for the localhost. This is used as a default
        /// host name thoughout this libary.
        /// </summary>
        public const string LocalHostName = "localhost";

        /// <summary>
        /// returns a listener with no message and responce actions.
        /// it will use the defualt parameters.
        /// </summary>
        /// <returns>A default listener</returns>
        public static ListenerSocket GetListener()
        {
            return new ListenerSocket();
        }

        /// <summary>
        /// gets a listener soket were it is possible to set the portnumber and 
        /// give it a object that can recive messages from it.
        /// </summary>
        /// <param name="serviceName">
        /// A string represention of the port you want to use.
        /// </param>
        /// <param name="messageHost">
        /// A object to handle messages from this device
        /// </param>
        /// <returns>
        /// A listener socket object
        /// </returns>
        public static ListenerSocket GetListener(string serviceName, IDisplayMessage messageHost)
        {
            return new ListenerSocket(serviceName, messageHost);
        }

        /// <summary>
        /// Gets a connection using localhost 
        /// as the host and the default portnumber
        /// </summary>
        /// <returns>
        /// Returns a default connection
        /// </returns>
        public static Connection GetConnection()
        {
            return new Connection();
        }

        /// <summary>
        /// Gets a new connection object that may communicate messages
        /// and allows for the the port and host name to be set
        /// </summary>
        /// <param name="serviceName">
        /// A string that represents a port number
        /// </param>
        /// <param name="hostName">
        /// A string that represents a host name
        /// </param>
        /// <param name="messageHost">
        /// A object implemening a IDisplayMessage Interface that will
        /// be able to communicate with this libary.
        /// </param>
        /// <returns>
        /// A new connection object
        /// </returns>
        public static Connection GetConnection(string serviceName, string hostName, IDisplayMessage messageHost)
        {
            return new Connection(serviceName, hostName, messageHost);
        }

        /// <summary>
        /// Gets a new connection object that may communicate messages
        /// </summary>
        /// <param name="MessageHostingService">
        /// A object implemening a IDisplayMessage Interface that will
        /// be able to communicate with this libary.
        /// </param>
        /// <returns>
        /// A new connection object
        /// </returns>
        public static Connection GetConnection(IDisplayMessage MessageHostingService)
        {
            return new Connection(MessageHostingService);
        }

        public static ListenerSocket GetListener(IDisplayMessage MessageHostingService)
        {
            return new ListenerSocket(MessageHostingService);
        }
    }
}
