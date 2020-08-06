using System.Net.Sockets;
using NetworkingLibaryStandard;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class HugeDataStucureNetworkingPackets : ITCPResponder, IContinuousMessageHandeler
    {
        private object obj = new Object();

        private Queue<KeyValuePair<string, string>> packetData = new Queue<KeyValuePair<string, string>>();

        public void AddData(string tagName, string data)
        {
            lock (obj)
            {
                if (tagName != null || data != null)
                {
                    packetData.Enqueue(new KeyValuePair<string, string>(tagName.ToLower(), data));
                }
            }
        }

        public string CompileOutputPackage()
        {
            lock (obj)
            {
                if (packetData.Count == 0)
                {
                    return Tags.Encapsulate(Tags.tageNames[(int)Tags.indexs.NoData], "");
                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    while (packetData.Count != 0)
                    {
                        KeyValuePair<string, string> keyval = packetData.Dequeue();
                        sb.Append(Tags.Encapsulate(keyval.Key, keyval.Value));
                    }
                    return sb.ToString();
                }
            }
        }

        public void Compile(string data)
        {
            if (Tags.Encapsulate(Tags.tageNames[(int)Tags.indexs.NoData], "").Equals(data))
            {
                Console.WriteLine("No Data In package");
            }
            else
            {
                foreach(string tagName in Tags.tageNames)
                {
                    string completedStart = Tags.MakeStartTag(tagName);

                    int start = data.IndexOf(completedStart, StringComparison.Ordinal);
                    if (start > -1)
                    {
                        int end = data.IndexOf(Tags.MakeEndTag(tagName), StringComparison.Ordinal);

                        // if the end dosn't come before or after the start tag
                        if (end > start)
                        {
                            Console.WriteLine(tagName);
                        }
                    }
                }
            }
        }

        public void Respond(NetworkStream stream, string incommingData)
        {
            Console.WriteLine(incommingData);

            Compile(incommingData);

            // send back the file
            // convert the sting into a byte array
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(CompileOutputPackage());

            // Send back a response.
            stream.Write(msg, 0, msg.Length);
        }

        public string RespondTo(string messageRecived)
        {
            Console.WriteLine(messageRecived);

            Compile(messageRecived);

            return CompileOutputPackage();
        }
    }
}
