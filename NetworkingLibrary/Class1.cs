using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingLibrary
{
    public class NetworkingLibaryCore
    {

        public const string DefaultServiceName = "22112";

        public const string LocalHostName = "localhost";

        public static ListenerSocket GetListener()
        {
            return new ListenerSocket();
        }

        public static ListenerSocket GetListener(string serviceName, IDisplayMessage messageHost)
        {
            return new ListenerSocket(serviceName, messageHost);
        }

        public static Connection GetConnection()
        {
            return new Connection();
        }

        public static Connection GetConnection(string serviceName, string hostName, IDisplayMessage messageHost)
        {
            return new Connection(serviceName, hostName, messageHost);
        }

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
