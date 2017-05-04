using CodeSmeller.Analyzers;
using CodeSmeller.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CodeSmeller.Tests.Analyzers
{
    [TestClass]
    public class StatementCounterTests
    {
        static dynamic _report;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            const string file = @"TestFiles\Analyzers\StatementCounter\TestCases.cs";
            var analyzer = new StatementCounter();
            var methods = file.Descendants<MethodDeclarationSyntax>();
            methods.ForEach(m => analyzer.Analyze(m, file));

            _report = JObject.Parse(analyzer.Report());
        }

        [TestMethod]
        public void ShouldReportTotalNumberOfMethods()
        {
            Assert.AreEqual(3, (int)_report.stats.methodCount);
        }

        [TestMethod]
        public void ShouldReportAverageNumberOfStatements()
        {
            Assert.AreEqual(4, (int)_report.stats.avgStatementsPerMethod);
        }

        [TestMethod]
        public void ShouldReportNumberOfHighStatementMethods()
        {
            Assert.AreEqual(1, (int)_report.stats.highStatementMethods);
        }

        [TestMethod]
        public void ShouldIgnoreEmptyMethods()
        {
            Assert.AreEqual(3, (int)_report.stats.methodCount);
        }
    }
}
