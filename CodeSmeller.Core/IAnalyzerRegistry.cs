using System.Collections.Generic;

namespace CodeSmeller.Core
{
    public interface IAnalyzerRegistry
    {
        List<INamespaceAnalyzer> NamespaceAnalyzers { get; }
        List<IClassAnalyzer> ClassAnalyzers { get; }
        List<IMethodAnalyzer> MethodAnalyzers { get; }
    }
}
