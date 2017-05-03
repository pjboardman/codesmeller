using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeSmeller.Core
{
    public interface IAnalyzerRegistry
    {
        List<IAnalyzer<NamespaceDeclarationSyntax>> NamespaceAnalyzers { get; }
        List<IAnalyzer<ClassDeclarationSyntax>> ClassAnalyzers { get; }
        List<IAnalyzer<MethodDeclarationSyntax>> MethodAnalyzers { get; }
        List<IAnalyzer> GetAll();
    }
}
