using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkingLibaryStandard;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //TCPListener listener = new TCPListener();
            TCPClient client = new TCPClient("10.160.99.76");

            //listener.Start();

            client.Start();

            client.Send("Hello World");

            /*for(int i = 0; i < 100; i++)
            {
                client.Send(getRandomCoords());
            }*/

            Console.ReadKey();

            //client.Close();
            //listener.Close();
        }

        static string getRandomCoords()
        {
            Random random = new Random();
            string output = "";
            
            for(int i = 0; i < 6; i++)
            {
                if (i != -1)
                {
                    output += ", ";
                }

                output += random.Next(360);
            }

            return output;
        }
    }
}
