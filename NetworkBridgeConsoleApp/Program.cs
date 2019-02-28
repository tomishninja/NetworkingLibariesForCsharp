﻿using NetworkingLibaryStandard;
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
            Console.WriteLine(WriteWelcomeMessage());
            string userInput = "";

            while (IsQuit(userInput))
            {
                Console.WriteLine("please enter a command:");
                userInput = Console.ReadLine().ToLower().Trim();

                switch (userInput)
                {
                    case "-help":
                        Console.WriteLine(WriteHelpMessage());
                        break;
                    case "-frameworks":
                        Console.WriteLine(WriteFrameWorksMessage());
                        break;
                    case "-start":
                        userInput = RunBridgingApplication();
                        break;
                    default:
                        if (userInput.Substring(0, 10).Equals("-start now"))
                        {
                            // Run faster Program
                            userInput = StartAll(userInput);
                        }
                        else
                        {
                            Console.WriteLine(WriteNoCommandFoundString(userInput));
                        }
                        break;
                }
            }
        }

        static string RunBridgingApplication()
        {
            // string value to hold the users inputs in
            string userInput = null;

            // bool value to tell the system when to repete a loop
            bool invalidFlag = false;

            // get the users input from the feild
            do
            {
                Console.WriteLine("Please enter the HostName or valid framework for the source of the packets you wish to use");
                userInput = Console.ReadLine().ToLower().Trim();

                if (IsFrameWork(userInput))
                {
                    char charinput = 'a';

                    // determine what framework and what should be done about it
                    if (userInput.Equals(Frameworks[0].ToLower()))
                    {
                        // this will be the LSL layer.
                        do
                        {
                            Console.WriteLine("Do you want to run a inlet(i) or a outlet(o)");
                            charinput = Console.ReadLine().Trim().ToCharArray()[0];

                            if (charinput == 'i')
                            {
                                new LSLInputConsoleWalkthough().Start();
                            }
                            else if (charinput == 'o')
                            {

                            }
                            else
                            {
                                Console.WriteLine("\'" + charinput + "\' is not a valid input");
                            }
                        } while (charinput != 'i' && charinput != 'o');

                    }
                    else
                    {
                        // this should never be reached if it is you need to work out what to do.
                    }
                }
                else if (System.Net.IPAddress.TryParse(userInput, out System.Net.IPAddress ipaddress))
                {
                    // the the user entered a valid ip address now continue to ask for a port number
                    do
                    {
                        // 
                        Console.WriteLine("Please enter the Port Number to listen to");
                        userInput = Console.ReadLine().ToLower().Trim();

                        if (int.TryParse(userInput, out int portNumber) && portNumber > -1)
                        {
                            // Start working with the connection
                            //TODO: implement the networking 
                        }
                        else if (IsQuit(userInput))
                        {
                            // get out of this
                            return userInput;
                        }
                        else
                        {
                            // this entry is in valid
                            invalidFlag = true;
                            Console.WriteLine("\"" + userInput + "\" is not a valid portnumber");
                        }
                    } while (invalidFlag);
                    // invalid flag must equal false to leave
                }
                else if (IsQuit(userInput))
                {
                    return userInput;
                }
            } while (invalidFlag);

            return null;
        }

        static bool IsQuit(string input)
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

        /// <summary>
        /// Writes a welcome message for the app. 
        /// This will be the first thing users see when they load this program
        /// </summary>
        /// <returns>
        /// retuns a string version of the welcome message
        /// </returns>
        static string WriteWelcomeMessage()
        {
            string welcomeMessage = "";

            welcomeMessage += "Welcome to the network bridging app.\n";
            welcomeMessage += "write \"-help\" to recive help. and a list of commands";
            welcomeMessage += "write \"-frameworks\" to see a list of avalible frameworks this program is functional with";
            welcomeMessage += "and descriptions about what they are for\n";
            welcomeMessage += "write \"-start\" to begin the briging process";
            welcomeMessage += "write \"-q\" or \"-quit to exit the application";

            return welcomeMessage;
        }

        static string StartAll(string userInput)
        {
            // veriables
            LSLFramework.LSLInletFloats inlet = null;
            NetworkingLibaryStandard.UDPClient outlet = null;

            string[] options = userInput.Split();

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
                            LSLInputConsoleWalkthough lslInputHelper = new LSLInputConsoleWalkthough();

                            // get the new input
                            inlet = lslInputHelper.BuildInlet(
                                        options[++cindex], options[++cindex], int.Parse(options[++cindex]),
                                        options[++cindex].Equals("-D"), true);

                            if (inlet == null)
                            {
                                return userInput;
                            }
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            return userInput;
                        }
                    case "udp":
                        //TODO run UDP listener
                        break;
                }
            }

            // the the current C index equals the d move forward one to get to the next marker
            if (options[cindex].Equals("-d"))
                cindex++;

            // work out the outlet details
            switch (options[cindex])
            {
                case "lsl":
                //TODO run LSL

                case "udp":
                    // Run UDP client
                    try
                    {
                        outlet = new UDPClient(int.Parse(options[++cindex]), options[++cindex]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return userInput;
                    }

                    break;
            }

            if (outlet != null && inlet != null)
            {
                inlet.Start();

                outlet.Start();


                Console.ReadKey();

                inlet.Stop();

                outlet.Disconnect();
            }

            return userInput;
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
