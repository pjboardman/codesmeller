using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeSmeller.Core
{
    public interface IClassAnalyzer : IAnalyzer
    {
        void Analyze(ClassDeclarationSyntax syntax);
    }
}
