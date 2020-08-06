using System;
using NetworkingLibaryStandard;

namespace SimpleSendMessage
{
    // A really simple program that is aimed at testing input for the system by allowing easy transactions.
    // as this is a prototype program it has no error checking code and it will crash if anything is unexpected,
    // in its current state
    class Program
    {
        static void Main(string[] args)
        {
            int portNumber = 22114;

            string message = "{\"Items\": [{\"TableHorizontal\": 500.0, \"Markers\": [{\"color\": {\"y\": 0.0, \"x\": 1.0, \"z\": 0.0, \"w\": 1.0}, \"id\": 2, \"pos\": {\"y\": -1.113050103187561, \"x\": -0.15132655203342438, \"z\": -0.17481088638305664}, \"name\": \"\"}, {\"color\": {\"y\": 0.0, \"x\": 0.0, \"z\": 1.0, \"w\": 1.0}, \"id\": 3, \"pos\": {\"y\": -1.0898972749710083, \"x\": -0.08224737644195557, \"z\": -0.17592275142669678}, \"name\": \"\"}, {\"color\": {\"y\": 1.0, \"x\": 0.0, \"z\": 0.0, \"w\": 1.0}, \"id\": 4, \"pos\": {\"y\": -1.1691724061965942, \"x\": -0.07297906279563904, \"z\": -0.17254739999771118}, \"name\": \"\", \"TableVertical\": -500.0}]}]}";

            UDPClient client = new UDPClient(portNumber);
            client.Start();
            client.Send(message);

            Console.WriteLine("Message Has been sent press any key to turn off close the socket and quit this program");
            Console.ReadKey();

            client.Close();
        }
    }
}
