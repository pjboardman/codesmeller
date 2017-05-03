using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections;

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
            var root = TreeHelper.GetRoot(file);

            var namespaces = TreeHelper.GetDescendants<NamespaceDeclarationSyntax>(root);
            if (namespaces.Any(x => IsTest(x))) return;

            AnalyzeNamespaces(namespaces, file);
            AnalyzeClasses(root, file);
            AnalyzeMethods(root, file);
        }

        private bool IsTest(NamespaceDeclarationSyntax syntax)
        {
            var names = syntax.DescendantNodes().OfType<IdentifierNameSyntax>();

            return names.Any(x => x.Identifier.Text.ToLower().Contains("test"));
        }

        private void AnalyzeNamespaces(List<NamespaceDeclarationSyntax> namespaces, string file)
        {
            if (_registry.NamespaceAnalyzers == null || !_registry.NamespaceAnalyzers.Any()) return;

            namespaces.ForEach(n => _registry.NamespaceAnalyzers.ForEach(x => x.Analyze(n, file)));
        }

        private void AnalyzeClasses(SyntaxNode root, string file)
        {
            if (_registry.ClassAnalyzers == null || !_registry.ClassAnalyzers.Any()) return;

            var classes = TreeHelper.GetDescendants<ClassDeclarationSyntax>(root);
            classes.ForEach(c => _registry.ClassAnalyzers.ForEach(analyzer => analyzer.Analyze(c, file)));
        }

        private void AnalyzeMethods(SyntaxNode root, string file)
        {
            if (_registry.MethodAnalyzers == null || !_registry.MethodAnalyzers.Any()) return;

            var methods = TreeHelper.GetDescendants<MethodDeclarationSyntax>(root);
            methods.ForEach(method => _registry.MethodAnalyzers.ForEach(analyzer => analyzer.Analyze(method, file)));
        }
    }
}
