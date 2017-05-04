using System;

namespace CodeSmeller.Tests.TestFiles.Analyzers.StatementCounter
{
    class TestCases
    {
        void One()
        {
            var x = 0 + 1;
            if (true)
            {
                var y = x * 3;
            }

            Console.WriteLine("one");
        }

        void Two()
        {
            One();
            Console.WriteLine("Two");
        }

        void Three()
        {
            if (1 > 0)
            {
                var a = 1;
                var b = 1;
                var c = 2;
                Two();
                if (a > b) Console.WriteLine("no");
            }

            Console.WriteLine("three");
        }

        void Empty() { }

    }
}
