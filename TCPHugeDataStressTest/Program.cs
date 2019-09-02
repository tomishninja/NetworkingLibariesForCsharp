using NetworkingLibaryStandard;
using System;

namespace TCPHugeDataStressTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPListener listener = new TCPListener();
            TCPClient client = new TCPClient("127.0.0.1");

            listener.Start();
            client.Start();

            client.Send(System.IO.File.ReadAllText(
                @"D:\\ThomasClarke\\Git\\NetworkingLibariesForCsharp\\TCPHugeDataStressTest\\rabbit.ply"));
            

            Console.WriteLine(listener.data);

            using (System.IO.StreamWriter sw = System.IO.File.CreateText(
                @"D:\\ThomasClarke\\Git\\NetworkingLibariesForCsharp\\TCPHugeDataStressTest\\programOutput.txt"))
            {
                sw.WriteLine(listener.data);
            }
            //client.Close();
            Console.ReadKey();

            listener.Close();
        }


    }
}
