using System;
using System.Collections.Generic;
using System.Linq;
using LSLFramework;

namespace NetworkBridgeConsoleApp.NetworkWrappers
{
    public abstract class AbstractLSLConsoleWalkThough : IDisplayMessage, INetworkObject
    {
        public const string UseDefault = "enter nothing to recive default value";
        public const string SeeDefault = "enter default to view current default value";

        public const string DefaultModifier = "default";

        public DataManagement.IDataStore<String> DataManager
        {
            get;
            internal set;
        }

        public static object DisplayMessageLock = new object();
        public virtual void DisplayMessage(string message)
        {
            lock (DisplayMessageLock)
            {
                Console.WriteLine(message);
            }
        }

        public abstract void Start();
        public abstract void Stop();
    }
}
