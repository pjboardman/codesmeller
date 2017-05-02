using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        private Mock<IAnalyzer<NamespaceDeclarationSyntax>> _namespaceAnalyzer;
        private Mock<IAnalyzer<ClassDeclarationSyntax>> _classAnalyzer;
        private Mock<IAnalyzer<MethodDeclarationSyntax>> _methodAnalyzer;
        private Mock<Smeller> _smeller;
        private RepositoryProcessor _processor;

        [TestInitialize]
        public void Setup()
        {
            _namespaceAnalyzer = new Mock<IAnalyzer<NamespaceDeclarationSyntax>>();
            _classAnalyzer = new Mock<IAnalyzer<ClassDeclarationSyntax>>();
            _methodAnalyzer = new Mock<IAnalyzer<MethodDeclarationSyntax>>();
            
            _registry = new Mock<IAnalyzerRegistry>();
            _registry.Setup(x => x.NamespaceAnalyzers).Returns(new List<IAnalyzer<NamespaceDeclarationSyntax>> { _namespaceAnalyzer.Object});
            _registry.Setup(x => x.ClassAnalyzers).Returns(new List<IAnalyzer<ClassDeclarationSyntax>> { _classAnalyzer.Object });
            _registry.Setup(x => x.MethodAnalyzers).Returns(new List<IAnalyzer<MethodDeclarationSyntax>> { _methodAnalyzer.Object });

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
