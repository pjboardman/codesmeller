using CodeSmeller.Core;
using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeSmeller.Smell
{
    internal class DuctTapeRegistry : IAnalyzerRegistry
    {
        List<IAnalyzer<NamespaceDeclarationSyntax>> IAnalyzerRegistry.NamespaceAnalyzers => throw new NotImplementedException();

        List<IAnalyzer<ClassDeclarationSyntax>> IAnalyzerRegistry.ClassAnalyzers => throw new NotImplementedException();

        List<IAnalyzer<MethodDeclarationSyntax>> IAnalyzerRegistry.MethodAnalyzers => throw new NotImplementedException();

        internal DuctTapeRegistry()
        {
            RegisterNamespaceAnalyzers();
            RegisterClassAnalyzers();
            RegisterMethodAnalyzers();
        }

        private void RegisterNamespaceAnalyzers()
        {
            throw new NotImplementedException();
        }

        private void RegisterClassAnalyzers()
        {
            throw new NotImplementedException();
        }

        private void RegisterMethodAnalyzers()
        {
            throw new NotImplementedException();
        }
    }
}
