using CodeSmeller.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace CodeSmeller.Analyzers
{
    public class DomainFinder : IAnalyzer<ClassDeclarationSyntax>
    {
        ConcurrentDictionary<string, int> _data;
        //or?
        //ConcurrentDictionary<QualifiedNameSyntax, int> _data;
        const int PropertyCountThreshhold = 3;

        public DomainFinder()
        {
            Initialize();
        }

        public void Initialize()
        {
            _data = new ConcurrentDictionary<string, int>();
        }

        public void Analyze(ClassDeclarationSyntax syntax, string file)
        {
            int methodCount = syntax.Descendants<MethodDeclarationSyntax>().Count;
            int propertyCount = syntax.Descendants<PropertyDeclarationSyntax>().Count;

            if (propertyCount > methodCount || propertyCount >= PropertyCountThreshhold)
            {
                string namespaceName = GetNamespace(syntax);
                _data.AddOrUpdate(namespaceName, 1, (k, v) => v += 1);
            }
        }

        //return QualifiedNameSyntax?
        private string GetNamespace(ClassDeclarationSyntax syntax)
        {
            NamespaceDeclarationSyntax namespaceSyntax = FindNamespace(syntax);

            if (namespaceSyntax == null) return null;

            var name = namespaceSyntax.Descendants<QualifiedNameSyntax>().First();

            throw new NotImplementedException("finish me");

        }

        private static NamespaceDeclarationSyntax FindNamespace(ClassDeclarationSyntax syntax)
        {
            NamespaceDeclarationSyntax namespaceSyntax = null;
            var child = (SyntaxNode)syntax;

            while (namespaceSyntax == null || child != null)
            {
                if (child.Parent is NamespaceDeclarationSyntax)
                {
                    namespaceSyntax = (NamespaceDeclarationSyntax)syntax.Parent;
                }
                else
                {
                    child = child.Parent;
                }
            }

            return namespaceSyntax;
        }

        public string Report()
        {
            throw new NotImplementedException();
        }

        public string Summarize()
        {
            throw new NotImplementedException();
        }
    }
}
