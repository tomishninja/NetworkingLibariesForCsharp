using System;

namespace NetworkBridgeConsoleApp
{
    public class LSLInputConsoleWalkthough : AbstractLSLConsoleWalkThough
    {
        bool DisplayData = false;

        bool StoreData = false;


        public LSLInputConsoleWalkthough()
        {
        }

        public override bool Start()
        {
            string prop = null;
            string value = null;
            int arrayLength = -1;
            LSLFramework.LSLInletFloats inlet;

            while (prop == null)
            {
                Console.WriteLine(UseDefault);
                Console.WriteLine(SeeDefault);
                Console.WriteLine("Please choose a prop for the inlet, ");

                string input = Console.ReadLine().Trim().ToLower();

                if (DefaultModifier.Equals(input))
                {
                    // TODO: show constant default.
                    Console.WriteLine(LSLFramework.LSLInletFloats.DefaultProp);
                }
                else if (input != null && !input.Equals(""))
                {
                    prop = input;
                }
                else
                {
                    prop = LSLFramework.LSLInletFloats.DefaultProp;
                }
            }

            Console.WriteLine("Prop is now set to: \"" + prop + "\"");

            while (value == null)
            {
                Console.WriteLine(UseDefault);
                Console.WriteLine(SeeDefault);
                Console.WriteLine("Please choose a value for the inlet\'s prop, ");

                string input = Console.ReadLine().Trim().ToLower();

                if (DefaultModifier.Equals(input))
                {
                    // TODO: show constant default.
                    Console.WriteLine(LSLFramework.LSLInletFloats.DefaultValue);
                }
                else if (input != null && !input.Equals(""))
                {
                    value = input;
                }
                else
                {
                    value = LSLFramework.LSLInletFloats.DefaultValue;
                }
            }

            Console.WriteLine("Value is now set to: \"" + value + "\"");

            do
            {
                Console.WriteLine(UseDefault);
                Console.WriteLine(SeeDefault);
                Console.WriteLine("What size should the float array of data be?");

                string input = Console.ReadLine().Trim().ToLower();
                int output = 0;

                // based on the users input decided what to do
                if (DefaultModifier.Equals(input))
                {
                    // TODO: show constant default.
                    Console.WriteLine(LSLFramework.LSLInletFloats.DefaultValue);
                }
                else if (input == null && input.Equals(""))
                {
                    arrayLength = LSLFramework.AbstractLSLObject.defaultArrayLength;
                }
                else if (int.TryParse(input, out output))
                {
                    arrayLength = output;
                }
            } while (arrayLength < 0);

            char cinput_TransferData;
            // give the system the option to choose how to were the data goes
            do
            {
                Console.WriteLine("Do you want to transfer this data to another stream?");
                cinput_TransferData = Console.ReadLine().Trim().ToLower().ToCharArray()[0];
            } while (cinput_TransferData != 'y' && cinput_TransferData != 'n');


            char cinput_DisplayData;
            do
            {
                Console.WriteLine("Do you want to see this data on the console enter y (yes) or n (no)");
                cinput_DisplayData = Console.ReadLine().Trim().ToLower().ToCharArray()[0];
            } while (cinput_DisplayData != 'y' && cinput_DisplayData != 'n');

            inlet = BuildInlet(prop, value, arrayLength, cinput_TransferData == 'y', cinput_DisplayData == 'y');

            Console.WriteLine("Value is now set to: \"" + value + "\"");

            Console.WriteLine("Now starting LSL inlet");

            // run LSL
            inlet.Start();

            Console.WriteLine("Press any key to stop lsl");
            Console.ReadKey();

            // stop the lsl after the user requests it
            inlet.Stop();

            return true;
        }

        public LSLFramework.LSLInletFloats BuildInlet(string prop, string value, int arrayLength, bool display, bool store)
        {
            LSLFramework.LSLInletFloats inlet;

            this.DisplayData = display;
            this.StoreData = store;

            // TODO set up a briding application here
            if (store && display)
            {
                // TODO: Transfer data and display it 
                DataManager = new DataManagement.SyncronisedQueueDataStore<string>(null);
                inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength, this);
            }
            else if (store)
            {
                DataManager = new DataManagement.SyncronisedQueueDataStore<string>(null);
                inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength, this);
            }
            if (display)
            {
                inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength, this);
            }
            else
            {
                // this option the user chose no to both options
                inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength);
            }

            return inlet;
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
    }
}
