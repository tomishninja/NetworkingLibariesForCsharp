using System.Net.Sockets;
using NetworkingLibaryStandard;
using System;

namespace ConsoleApp1
{
    public class TCPEchoResponderForHugeAmountOfData : ITCPResponder, IContinuousMessageHandeler
    {
        private string expectedImage;

        public TCPEchoResponderForHugeAmountOfData(string data)
        {
            expectedImage = data;
        }

        public void Respond(NetworkStream stream, string data)
        {
            Console.WriteLine("Client: Data is correct: " + data.Equals(expectedImage));

            // send back the file
            // convert the sting into a byte array
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

            // Send back a response.
            stream.Write(msg, 0, msg.Length);
        }

        public string RespondTo(string messageRecived)
        {
            Console.WriteLine("Listener: Data is correct: " + messageRecived.Equals(expectedImage));

            return messageRecived;
        }
    }
}
