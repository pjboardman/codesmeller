using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeSmeller.Core
{
    public class Smeller
    {
        private IAnalyzerRegistry _registry;

        public Smeller(IAnalyzerRegistry registry)
        {
            _registry = registry;
        }

        public virtual void Smell(string file)
        {
            var code = File.ReadAllText(file);
            var tree = SyntaxFactory.ParseSyntaxTree(code);

            var namespaces = GetDescendants<NamespaceDeclarationSyntax>(tree);
            if (namespaces.Any(x => IsTest(x))) return;
            namespaces.ForEach(n => AnalyzeNamespace(n));

            AnalyzeClasses(tree);
            AnalyzeMethods(tree);
        }

        List<T> GetDescendants<T>(SyntaxTree tree)
        {
            return tree.GetRoot().DescendantNodes().OfType<T>().ToList();
        }

        private bool IsTest(NamespaceDeclarationSyntax syntax)
        {
            var names = syntax.DescendantNodes().OfType<IdentifierNameSyntax>();

            return names.Any(x => x.Identifier.Text.ToLower().Contains("test"));
        }

        private void AnalyzeNamespace(NamespaceDeclarationSyntax namespaceSyntax)
        {
            if (_registry.NamespaceAnalyzers == null || !_registry.NamespaceAnalyzers.Any()) return;

            _registry.NamespaceAnalyzers.ForEach(x => x.Analyze(namespaceSyntax));
        }

        private void AnalyzeClasses(SyntaxTree tree)
        {
            if (_registry.ClassAnalyzers == null || !_registry.ClassAnalyzers.Any()) return;

            var classes = GetDescendants<ClassDeclarationSyntax>(tree);
            classes.ForEach(c => _registry.ClassAnalyzers.ForEach(analyzer => analyzer.Analyze(c)));
        }
        private void AnalyzeMethods(SyntaxTree tree)
        {
            if (_registry.MethodAnalyzers == null || !_registry.MethodAnalyzers.Any()) return;

            var methods = GetDescendants<MethodDeclarationSyntax>(tree);
            methods.ForEach(method => _registry.MethodAnalyzers.ForEach(analyzer => analyzer.Analyze(method)));
        }
    }
}
