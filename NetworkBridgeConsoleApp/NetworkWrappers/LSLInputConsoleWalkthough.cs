using System;

namespace NetworkBridgeConsoleApp.NetworkWrappers
{
    public class LSLInputConsoleWalkthough : AbstractLSLConsoleWalkThough
    {
        bool DisplayData = false;

        bool StoreData = false;

        LSLFramework.LSLInletFloats Inlet;

        public LSLInputConsoleWalkthough(string prop, string value, int arrayLength, bool display, bool store)
        {
            this.DisplayData = display;
            this.StoreData = store;

            // TODO set up a briding application here
            if (store && display)
            {
                // TODO: Transfer data and display it 
                DataManager = new DataManagement.SyncronisedQueueDataStore<string>(null);
                Inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength, this);
            }
            else if (store)
            {
                DataManager = new DataManagement.SyncronisedQueueDataStore<string>(null);
                Inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength, this);
            }
            if (display)
            {
                Inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength, this);
            }
            else
            {
                // this option the user chose no to both options
                Inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength);
            }
        }

        public override void DisplayMessage(string message)
        {
            lock (DisplayMessageLock)
            {
                if (StoreData && message != null)
                {
                    this.DataManager.Add(message);
                }
                if (DisplayData)
                {
                    Console.WriteLine(message);
                }
            }
        }

        public override void Start()
        {
            // this method will run lsl in its own thread
            this.Inlet.Start();
        }

        public override void Stop()
        {
            this.Inlet.Stop();
        }
    }
}
