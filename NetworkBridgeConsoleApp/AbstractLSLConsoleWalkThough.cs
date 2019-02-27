using System;
using System.Collections.Generic;
using System.Linq;
using LSLFramework;

namespace NetworkBridgeConsoleApp
{
    public abstract class AbstractLSLConsoleWalkThough : IDisplayMessage
    {
        public const string UseDefault = "enter nothing to recive default value";
        public const string SeeDefault = "enter default to view current default value";

        public const string DefaultModifier = "default";


        public static object obj = new object();
        public void DisplayMessage(string message)
        {
            lock (obj)
            {
                Console.WriteLine(message);
            }
        }

        public abstract bool Start();
    }
}
