using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeSmeller.Core
{
    public interface IMethodAnalyzer : IAnalyzer
    {
        void Analyze(MethodDeclarationSyntax syntax);
    }
}
