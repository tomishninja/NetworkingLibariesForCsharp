using NetworkingLibaryStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkBridgeConsoleApp.NetworkWrappers
{
    class UDPOutlet : AbstractNetworkOutlet, INetworkObject
    {
        public readonly UDPClient Outlet = null;

        private bool keepThreadRunning = false;

        private Thread behaviouralThread = null;

        public const int millisecondsToWaitAfterFail = 10;


        public UDPOutlet(int portNumber, string hostName, DataManagement.IDataStore<string> dataStore) : base (dataStore)
        {
            Outlet = new UDPClient(portNumber, hostName);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Outlet.Start();

            // tell the main loop not to stop
            keepThreadRunning = true;

            // run the method that will keep the main loop runing
            behaviouralThread = new Thread(StartSendingFromDataStore);
            behaviouralThread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartSendingFromDataStore()
        {
            // this value will hold the next item to send off if it is valid
            string outputFromDS = "";

            // keep getting data while it exists
            while (keepThreadRunning)
            {
                // if there is any output send it off or else wait a while
                if (this.DataManager.TryGetNext(out outputFromDS))
                {
                    Outlet.Send(outputFromDS);
                }
                else
                {
                    // just wait a short while for data to pile up
                    Thread.Sleep(millisecondsToWaitAfterFail);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            this.keepThreadRunning = false;

            // wait for the loop to end
            Thread.Sleep(millisecondsToWaitAfterFail * 2);

            this.Outlet.Close();
        }
    }
}
