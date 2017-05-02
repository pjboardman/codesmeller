using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
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
            var analyzer = new Mock<IAnalyzer<NamespaceDeclarationSyntax>>();
            _registry.SetupGet(x => x.NamespaceAnalyzers).Returns(new List<IAnalyzer<NamespaceDeclarationSyntax>> { analyzer.Object });

            _smeller.Smell(@"TestFiles\Test\NotAnalyzed.cs");

            analyzer.Verify(x => x.Analyze(It.IsAny<NamespaceDeclarationSyntax>()), Times.Never);
        }

        [TestMethod]
        public void Should_Analayze_All_Namespaces()
        {
            var analyzer1 = new Mock<IAnalyzer<NamespaceDeclarationSyntax>>();
            var analyzer2 = new Mock<IAnalyzer<NamespaceDeclarationSyntax>>();
            _registry.SetupGet(x => x.NamespaceAnalyzers).Returns(new List<IAnalyzer<NamespaceDeclarationSyntax>> {
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
            var analyzer1 = new Mock<IAnalyzer<ClassDeclarationSyntax>>();
            var analyzer2 = new Mock<IAnalyzer<ClassDeclarationSyntax>>();
            _registry.SetupGet(x => x.ClassAnalyzers).Returns(new List<IAnalyzer<ClassDeclarationSyntax>> {
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
            var analyzer1 = new Mock<IAnalyzer<MethodDeclarationSyntax>>();
            var analyzer2 = new Mock<IAnalyzer<MethodDeclarationSyntax>>();
            _registry.SetupGet(x => x.MethodAnalyzers).Returns(new List<IAnalyzer<MethodDeclarationSyntax>> {
                analyzer1.Object,
                analyzer2.Object
            });

            _smeller.Smell(@"TestFiles\B\TestB1.cs");

            analyzer1.Verify(x => x.Analyze(It.IsAny<MethodDeclarationSyntax>()), Times.Exactly(2));
            analyzer2.Verify(x => x.Analyze(It.IsAny<MethodDeclarationSyntax>()), Times.Exactly(2));
        }
    }
}
