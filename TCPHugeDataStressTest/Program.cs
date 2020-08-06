using NetworkingLibaryStandard;
using System;

namespace TCPHugeDataStressTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPListener listener = new TCPListener();
            //TCPClient client = new TCPClient(22114, "127.0.0.1");

            listener.Start();
            //client.Start();

            //client.Send(System.IO.File.ReadAllText(
                //@"D:\\ThomasClarke\\Git\\NetworkingLibariesForCsharp\\TCPHugeDataStressTest\\rabbit.ply"));

            //client.Close();

            //using (System.IO.StreamWriter sw = System.IO.File.CreateText(
                //@"D:\\ThomasClarke\\Git\\NetworkingLibariesForCsharp\\TCPHugeDataStressTest\\programOutput.txt"))
            //{
                //sw.WriteLine(listener.data);
            //}

            Console.WriteLine(listener.data);

            Console.ReadKey();

            //listener.Close();
        }
    }
}
