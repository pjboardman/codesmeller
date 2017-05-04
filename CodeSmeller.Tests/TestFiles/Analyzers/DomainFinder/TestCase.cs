using System;
using System.Collections.Generic;
using System.Text;

namespace Test.NotTheDomain
{
    class AService
    {
        string SomeAttribute { get; }
        void DoSomething() { }
        void DoSomethingElse() { }
    }

    class AnotherService
    {
        void DoSomethingADifferentWay() { }
        void DoSomethingElseADifferentWay() { }
    }
}

namespace Test.Domain.CouldBeADomain
{
    class Thing
    {
        string Attribute { get; set; }
        int AnotherAttribute { get; set; }
        DateTime YetAnotherAttribute { get; set; }

        void DomainLogic() { }
        void MoreDomainLogic() { }
    }

    class AnotherThing
    {
        string Attribute { get; set; }
        int AnotherAttribute { get; set; }
        void DomainLogic() { }
    }
}

namespace Test.Domain.CouldAlsoBeADomain
{
    class Stuff
    {
        string Info { get; }
        int State { get; }
    }

    class MoreStuff
    {
        string Data { get; set; }
        bool Flag { get; }
    }
}

