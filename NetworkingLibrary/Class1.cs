using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingLibrary
{
    public class Class1
    {

        public const string DefaultServiceName = "22112";

        public const string LocalHostName = "localhost";

        public static ListenerSocket GetListener()
        {
            return new ListenerSocket();
        }

        public static ListenerSocket GetListener(string serviceName, iDisplayMessage messageHost)
        {
            return new ListenerSocket(serviceName, messageHost);
        }

        public static Connection GetConnection()
        {
            return new Connection();
        }

        public static Connection GetConnection(string serviceName, string hostName, iDisplayMessage messageHost)
        {
            return new Connection(serviceName, hostName, messageHost);
        }

        public static Connection GetConnection(iDisplayMessage MessageHostingService)
        {
            return new Connection(MessageHostingService);
        }

        public static ListenerSocket GetListener(iDisplayMessage MessageHostingService)
        {
            return new ListenerSocket(MessageHostingService);
        }
    }
}
