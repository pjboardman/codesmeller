using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeSmeller.Core
{
    public static class SyntaxTreeExtensions
    {
        public static SyntaxNode GetRoot(this string file)
        {
            var code = File.ReadAllText(file);
            var tree = SyntaxFactory.ParseSyntaxTree(code);
            return tree.GetRoot();
        }

        public static List<T> Descendants<T>(this string file)
        {
            var root = GetRoot(file);
            return root.Descendants<T>();
        }

        public static List<T> Descendants<T>(this SyntaxNode root)
        {
            return root.DescendantNodes().OfType<T>().ToList();
        }
    }
}
