using CodeSmeller.Core;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSmeller.Analyzers;

namespace CodeSmeller.Smell
{
    internal class DuctTapeRegistry : IAnalyzerRegistry
    {
        public List<IAnalyzer<NamespaceDeclarationSyntax>> NamespaceAnalyzers { get; set; } = new List<IAnalyzer<NamespaceDeclarationSyntax>>();

        public List<IAnalyzer<ClassDeclarationSyntax>> ClassAnalyzers { get; set; } = new List<IAnalyzer<ClassDeclarationSyntax>>();

        public List<IAnalyzer<MethodDeclarationSyntax>> MethodAnalyzers { get; set; } = new List<IAnalyzer<MethodDeclarationSyntax>>();

        internal DuctTapeRegistry()
        {
            RegisterNamespaceAnalyzers();
            RegisterClassAnalyzers();
            RegisterMethodAnalyzers();
        }

        private void RegisterNamespaceAnalyzers()
        {
        }

        private void RegisterClassAnalyzers()
        {
        }

        private void RegisterMethodAnalyzers()
        {
            MethodAnalyzers.Add(new GuardClauseAnalyzer());
            MethodAnalyzers.Add(new StatementCounter());
        }

        public List<IAnalyzer> GetAll()
        {
            return NamespaceAnalyzers.Select(x => (IAnalyzer)x)
                .Concat(ClassAnalyzers.Select(x => (IAnalyzer)x))
                .Concat(MethodAnalyzers.Select(x => (IAnalyzer)x))
                .ToList();
        }
    }
}
