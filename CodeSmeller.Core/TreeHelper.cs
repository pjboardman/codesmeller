using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeSmeller.Core
{
    public static class TreeHelper
    {

        public static SyntaxNode GetRoot(string file)
        {
            var code = File.ReadAllText(file);
            var tree = SyntaxFactory.ParseSyntaxTree(code);
            return tree.GetRoot();
        }

        public static List<T> GetDescendants<T>(SyntaxNode root)
        {
            return root.DescendantNodes().OfType<T>().ToList();
        }

        public static List<T> GetDescendants<T>(string file)
        {
            var root = GetRoot(file);
            return GetDescendants<T>(root);
        }
    }
}
