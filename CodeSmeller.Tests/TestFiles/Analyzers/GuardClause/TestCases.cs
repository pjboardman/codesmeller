using System;

namespace CodeSmeller.Tests.TestFiles.Analyzers.GuardClause
{
    class TestCases
    {

        void NoConditionals()
        {
            var a = 1 + 2;
            var b = 3 + 4;
            var c = a + b;
            var d = a * b;
            Console.WriteLine("testing");
        }

        void NestedInConditional()
        {
            var a = 1;

            if (a > 0)
            {
                var b = 3 + 4;
                var c = a + b;
                var d = a * b;
                Console.WriteLine("testing fully nested");
            }
        }

        void PartiallyNestedInConditional()
        {
            var a = 1;
            var b = 2;

            if (a > 0)
            {
                var c = a + b;
                var d = a * b;
                var e = a / b;
                Console.WriteLine("testing partially nested");
            }
        }

        void HasGuardClause()
        {
            var a = 1;

            if (a <= 0) return;

            var b = 3 + 4;
            var c = a + b;
            var d = a * b;
            Console.WriteLine("testing guard clause");
        }
    }
}
