using NetworkBridgeConsoleApp.NetworkWrappers;
using NetworkingLibaryStandard;
using System;

namespace NetworkBridgeConsoleApp
{
    class Program : NetworkingLibaryStandard.IDisplayMessage
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly static string[] Frameworks =
        {
            "LSL",
            "UDP"
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string userInput = null;
            userInput = args[0].ToLower().Trim();

            switch (userInput)
            {
                case "-help":
                    Console.WriteLine(WriteHelpMessage());
                    break;
                case "-frameworks":
                    Console.WriteLine(WriteFrameWorksMessage());
                    break;
                default:
                    if (userInput.Equals("-start"))
                    {
                        // Run faster Program
                        StartAll(args);
                    }
                    else
                    {
                        Console.WriteLine(WriteNoCommandFoundString(userInput));
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>false if it the quit symbol has been used or else it will return false</returns>
        static bool IsNotQuit(string input)
        {
            return input == null || !(input.Equals("-q") || input.Equals("-quit"));
        }

        static string WriteNoCommandFoundString(string currentCommand)
        {
            string noCommandMessage = "";

            noCommandMessage += "The command \"" + currentCommand + "\" isn't reconised by this program. ";
            noCommandMessage += "please try another command or type in the command -help for more guidance";

            return noCommandMessage;
        }

        static string WriteFrameWorksMessage()
        {
            string frameworksMessage = "";

            frameworksMessage += "TODO: write framework message";

            return frameworksMessage;
        }

        private static bool IsFrameWork(string input)
        {
            for (int i = 0; i < Frameworks.Length; i++)
            {
                if (input.Equals(Frameworks[i].ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        static string WriteHelpMessage()
        {
            string helpMessage = "";

            helpMessage += "TODO: write help message";

            return helpMessage;
        }

        static void StartAll(string[] args)
        {
            // veriables
            INetworkObject inlet = null;
            INetworkObject outlet = null;
            DataManagement.IDataStore<string> dataStore = null;

            string[] options = args;

            int cindex = 2;

            // determine the inlet
            if (IsFrameWork(options[cindex]))
            {
                switch (options[cindex])
                {
                    case "lsl":
                        // run LSL
                        try
                        {
                            // get the new input
                            inlet = new LSLInputConsoleWalkthough(
                                        options[++cindex], options[++cindex], int.Parse(options[++cindex]),
                                        options[++cindex].Equals("-D"), true);

                            if (inlet == null)
                            {
                                return;
                            }
                            else
                            {
                                dataStore = ((LSLInputConsoleWalkthough)inlet).DataManager;
                            }
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            return;
                        }
                    case "udp":
                        //TODO run UDP listener
                        break;
                }
            }

            // the the current C index equals the d move forward one to get to the next marker
            if (options[cindex].ToLower().Equals("-d"))
                cindex++;

            // work out the outlet details
            switch (options[cindex].ToLower())
            {
                case "lsl":
                //TODO run LSL

                case "udp":
                    // Run UDP client
                    try
                    {
                        outlet = new UDPOutlet(int.Parse(options[++cindex]), options[++cindex], dataStore);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }

                    break;
            }

            if (outlet != null && inlet != null)
            {
                inlet.Start();

                outlet.Start();

                Console.ReadKey();

                inlet.Stop();

                outlet.Stop();
                outlet.Stop();
            }

            return;
        }

        public static object DisplayMessageLock = new object();
        public void DisplayMessage(MessageHelper.MessageType type, string message)
        {
            lock (DisplayMessageLock)
            {
                Console.WriteLine(message);
            }
        }
    }
}
