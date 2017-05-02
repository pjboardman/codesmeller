using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeSmeller.Core
{
    public interface INamespaceAnalyzer : IAnalyzer
    {
        void Analyze(NamespaceDeclarationSyntax syntax);
    }
}
