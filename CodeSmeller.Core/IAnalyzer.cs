using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeSmeller.Core
{
    public interface IAnalyzer
    {
        string Summarize();
        string Report();
    }

    public interface IAnalyzer<T> : IAnalyzer where T: MemberDeclarationSyntax
    {
        void Analyze(T syntax);
    }
}
