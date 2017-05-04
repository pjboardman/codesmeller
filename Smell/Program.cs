using CodeSmeller.Core;
using System;

namespace CodeSmeller.Smell
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                ShowUsage();
                return;
            }

            var registry = new DuctTapeRegistry();
            var processor = new RepositoryProcessor(registry);
            processor.Process(args[0]);
        }

        private static void ShowUsage()
        {
            Console.WriteLine("\r\nCode Smeller Usage:");
            Console.WriteLine("smell [directory]");
        }
    }
}