using System;


namespace App2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Hello - no args");
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine($"arg[{i}] = {args[i]}");
                }
            }
            Console.WriteLine("Press a key to continue: ");
            Console.ReadLine();
        }
    }
}
