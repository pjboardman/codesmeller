using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using CodeSmeller.Core;
using Moq;

namespace CodeSmeller.Tests.Core
{
    [TestClass]
    public class SmellerTests
    {
        Mock<IAnalyzerRegistry> _registry;
        Smeller _smeller;

        [TestInitialize]
        public void Setup()
        {
            _registry = new Mock<IAnalyzerRegistry>();
            _smeller = new Smeller(_registry.Object);
        }

        [TestMethod]
        public void Should_Skip_Tests()
        {
            var analyzer = new Mock<INamespaceAnalyzer>();
            _registry.SetupGet(x => x.NamespaceAnalyzers).Returns(new List<INamespaceAnalyzer> { analyzer.Object });

            _smeller.Smell(@"TestFiles\Test\NotAnalyzed.cs");

            analyzer.Verify(x => x.Analyze(It.IsAny<NamespaceDeclarationSyntax>()), Times.Never);
        }

        [TestMethod]
        public void Should_Analayze_All_Namespaces()
        {
            var analyzer1 = new Mock<INamespaceAnalyzer>();
            var analyzer2 = new Mock<INamespaceAnalyzer>();
            _registry.SetupGet(x => x.NamespaceAnalyzers).Returns(new List<INamespaceAnalyzer> {
                analyzer1.Object,
                analyzer2.Object
            });

            _smeller.Smell(@"TestFiles\A\TestA2.cs");

            analyzer1.Verify(x => x.Analyze(It.IsAny<NamespaceDeclarationSyntax>()), Times.Exactly(2));
            analyzer2.Verify(x => x.Analyze(It.IsAny<NamespaceDeclarationSyntax>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Should_Analyze_All_Classes()
        {
            var analyzer1 = new Mock<IClassAnalyzer>();
            var analyzer2 = new Mock<IClassAnalyzer>();
            _registry.SetupGet(x => x.ClassAnalyzers).Returns(new List<IClassAnalyzer> {
                analyzer1.Object,
                analyzer2.Object
            });
            
            _smeller.Smell(@"TestFiles\A\TestA1.cs");

            analyzer1.Verify(x => x.Analyze(It.IsAny<ClassDeclarationSyntax>()), Times.Exactly(2));
            analyzer2.Verify(x => x.Analyze(It.IsAny<ClassDeclarationSyntax>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Should_Analyze_All_Methods()
        {
            var analyzer1 = new Mock<IMethodAnalyzer>();
            var analyzer2 = new Mock<IMethodAnalyzer>();
            _registry.SetupGet(x => x.MethodAnalyzers).Returns(new List<IMethodAnalyzer> {
                analyzer1.Object,
                analyzer2.Object
            });

            _smeller.Smell(@"TestFiles\B\TestB1.cs");

            analyzer1.Verify(x => x.Analyze(It.IsAny<MethodDeclarationSyntax>()), Times.Exactly(2));
            analyzer2.Verify(x => x.Analyze(It.IsAny<MethodDeclarationSyntax>()), Times.Exactly(2));
        }
    }
}
