using CodeSmeller.Analyzers;
using CodeSmeller.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeSmeller.Tests.Analyzers
{
    [TestClass]
    public class DomainFinderTests
    {
        [TestMethod]
        public void ShouldFindDomainCandidates()
        {
            const string file = @"TestFiles\Analyzers\DomainFinder\TestCases.cs";
            var classes = file.Descendants<ClassDeclarationSyntax>();
            var analyzer = new DomainFinder();

            classes.ForEach(c => analyzer.Analyze(c, file));
            var report = analyzer.Report();

            Assert.IsFalse(report.Contains("Test.NotTheDomain"));
            Assert.IsTrue(report.Contains("Test.Domain"));
            Assert.IsFalse(report.Contains("Test.Domain.CouldBeADomain"));
        }
    }
}
