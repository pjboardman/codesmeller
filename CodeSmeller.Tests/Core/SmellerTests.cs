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
        public void ShouldSkipTests()
        {
            const string file = @"TestFiles\Test\NotAnalyzed.cs";
            var analyzer = new Mock<IAnalyzer<NamespaceDeclarationSyntax>>();
            _registry.SetupGet(x => x.NamespaceAnalyzers).Returns(new List<IAnalyzer<NamespaceDeclarationSyntax>> { analyzer.Object });

            _smeller.Smell(file);

            analyzer.Verify(x => x.Analyze(It.IsAny<NamespaceDeclarationSyntax>(), file), Times.Never);
        }

        [TestMethod]
        public void ShouldAnalayzeAllNamespaces()
        {
            const string file = @"TestFiles\A\TestA2.cs";
            var analyzer1 = new Mock<IAnalyzer<NamespaceDeclarationSyntax>>();
            var analyzer2 = new Mock<IAnalyzer<NamespaceDeclarationSyntax>>();
            _registry.SetupGet(x => x.NamespaceAnalyzers).Returns(new List<IAnalyzer<NamespaceDeclarationSyntax>> {
                analyzer1.Object,
                analyzer2.Object
            });

            _smeller.Smell(file);

            analyzer1.Verify(x => x.Analyze(It.IsAny<NamespaceDeclarationSyntax>(), file), Times.Exactly(2));
            analyzer2.Verify(x => x.Analyze(It.IsAny<NamespaceDeclarationSyntax>(), file), Times.Exactly(2));
        }

        [TestMethod]
        public void ShouldAnalyzeAllClasses()
        {
            const string file = @"TestFiles\A\TestA1.cs";
            var analyzer1 = new Mock<IAnalyzer<ClassDeclarationSyntax>>();
            var analyzer2 = new Mock<IAnalyzer<ClassDeclarationSyntax>>();
            _registry.SetupGet(x => x.ClassAnalyzers).Returns(new List<IAnalyzer<ClassDeclarationSyntax>> {
                analyzer1.Object,
                analyzer2.Object
            });

            _smeller.Smell(file);

            analyzer1.Verify(x => x.Analyze(It.IsAny<ClassDeclarationSyntax>(), file), Times.Exactly(2));
            analyzer2.Verify(x => x.Analyze(It.IsAny<ClassDeclarationSyntax>(), file), Times.Exactly(2));
        }

        [TestMethod]
        public void ShouldAnalyzeAllMethods()
        {
            const string file = @"TestFiles\B\TestB1.cs";
            var analyzer1 = new Mock<IAnalyzer<MethodDeclarationSyntax>>();
            var analyzer2 = new Mock<IAnalyzer<MethodDeclarationSyntax>>();
            _registry.SetupGet(x => x.MethodAnalyzers).Returns(new List<IAnalyzer<MethodDeclarationSyntax>> {
                analyzer1.Object,
                analyzer2.Object
            });

            _smeller.Smell(file);

            analyzer1.Verify(x => x.Analyze(It.IsAny<MethodDeclarationSyntax>(), file), Times.Exactly(2));
            analyzer2.Verify(x => x.Analyze(It.IsAny<MethodDeclarationSyntax>(), file), Times.Exactly(2));
        }
    }
}
