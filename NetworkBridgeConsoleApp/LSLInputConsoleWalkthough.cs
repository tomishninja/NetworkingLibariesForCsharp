using System;

namespace NetworkBridgeConsoleApp
{
    public class LSLInputConsoleWalkthough : AbstractLSLConsoleWalkThough
    {
        public LSLInputConsoleWalkthough()
        {
        }

        public override bool Start()
        {
            string prop = null;
            string value = null;
            int arrayLength = -1;

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

            LSLFramework.LSLInletFloats inlet;

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

            // TODO set up a briding application here
            if (cinput_TransferData == 'y' && cinput_DisplayData == 'y')
            {
                // TODO: Transfer data and display it 
            }
            else if (cinput_TransferData == 'y')
            {
                // TODO: Transfer data and do not display it 
            }
            if (cinput_DisplayData == 'y')
            {
                inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength, this);
            }
            else
            {
                // this option the user chose no to both options
                inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength);
            }

            Console.WriteLine("Value is now set to: \"" + value + "\"");

            Console.WriteLine("Now starting LSL inlet");

            LSLFramework.LSLInletFloats inlet = new LSLFramework.LSLInletFloats(prop, value, arrayLength, this);

            // run LSL
            inlet.Start();

            Console.WriteLine("Press any key to stop lsl");
            Console.ReadKey();

            // stop the lsl after the user requests it
            inlet.Stop();

            return true;
        }
    }
}
