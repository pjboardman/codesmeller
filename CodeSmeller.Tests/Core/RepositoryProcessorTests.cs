using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CodeSmeller.Core;
using Moq;

namespace CodeSmeller.Tests.Core
{
    [TestClass]
    public class RepositoryProcessor_ProcessTests
    {
        private const string DIR = "TestFiles";
        private Mock<IAnalyzerRegistry> _registry;
        private Mock<INamespaceAnalyzer> _namespaceAnalyzer;
        private Mock<IClassAnalyzer> _classAnalyzer;
        private Mock<IMethodAnalyzer> _methodAnalyzer;
        private Mock<Smeller> _smeller;
        private RepositoryProcessor _processor;

        [TestInitialize]
        public void Setup()
        {
            _namespaceAnalyzer = new Mock<INamespaceAnalyzer>();
            _classAnalyzer = new Mock<IClassAnalyzer>();
            _methodAnalyzer = new Mock<IMethodAnalyzer>();
            
            _registry = new Mock<IAnalyzerRegistry>();
            _registry.Setup(x => x.NamespaceAnalyzers).Returns(new List<INamespaceAnalyzer> { _namespaceAnalyzer.Object});
            _registry.Setup(x => x.ClassAnalyzers).Returns(new List<IClassAnalyzer> { _classAnalyzer.Object });
            _registry.Setup(x => x.MethodAnalyzers).Returns(new List<IMethodAnalyzer> { _methodAnalyzer.Object });

            _smeller = new Mock<Smeller>(_registry.Object);
            _processor = new RepositoryProcessor(_registry.Object, _smeller.Object);
        }

        [TestMethod]
        public void Analyzes_AllFiles()
        {
            _processor.Process(DIR);

            _smeller.Verify(x => x.Smell($"{DIR}\\A\\TestA1.cs"));
            _smeller.Verify(x => x.Smell($"{DIR}\\A\\TestA2.cs"));
            _smeller.Verify(x => x.Smell($"{DIR}\\B\\TestB1.cs"));
        }

        [TestMethod]
        public void Requests_AllSummaries()
        {
            _processor.Process(DIR);

            _namespaceAnalyzer.Verify(x => x.Summarize());
            _classAnalyzer.Verify(x => x.Summarize());
            _methodAnalyzer.Verify(x => x.Summarize());
        }

        [TestMethod]
        public void Creates_Report()
        {
            _processor.Process(DIR);

            _namespaceAnalyzer.Verify(x => x.Report());
            _classAnalyzer.Verify(x => x.Report());
            _methodAnalyzer.Verify(x => x.Report());
        }
    }
}
