using System;

namespace Stage0 // Note: actual namespace depends on the project name.
{
     partial class Program
    {
        static void Main(string[] args)
        {
            Welcome0586();
            Welcome5404();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        static partial void Welcome5404();

        private static void Welcome0586()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application!", name);
        }
    }
}
