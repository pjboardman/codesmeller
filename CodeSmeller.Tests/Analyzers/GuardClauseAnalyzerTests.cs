using CodeSmeller.Analyzers;
using CodeSmeller.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSmeller.Tests.Analyzers
{
    [TestClass]
    public class GuardClauseAnalyzerTests
    {
        static dynamic _report;
        static string _summary;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            const string file = @"TestFiles\Analyzers\GuardClause\TestCases.cs";
            var analyzer = new GuardClauseAnalyzer();
            var methods = file.Descendants<MethodDeclarationSyntax>();
            methods.ForEach(m => analyzer.Analyze(m, file));

            _summary = analyzer.Summarize();
            _report = JObject.Parse(analyzer.Report());
        }

        [TestMethod]
        public void ShouldIgnoreMethodsWithoutConditionals()
        {
            Assert.AreEqual(3, (int)_report.stats.methodsAnalyzed);
        }

        [TestMethod]
        public void ShouldTrackMethodsWithExistingGuardClauses()
        {
            Assert.AreEqual(1, (int)_report.stats.methodsWithGuardClause);
        }

        [TestMethod]
        public void ShouldTrackMethodsThatCouldUseGuardClauses()
        {
            Assert.AreEqual(2, (int)_report.stats.candidates);
        }
    }
}
