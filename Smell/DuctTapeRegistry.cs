using CodeSmeller.Core;
using System;
using System.Collections.Generic;

namespace CodeSmeller.Smell
{
    internal class DuctTapeRegistry : IAnalyzerRegistry
    {
        public List<INamespaceAnalyzer> NamespaceAnalyzers => new List<INamespaceAnalyzer>();
        public List<IClassAnalyzer> ClassAnalyzers => new List<IClassAnalyzer>();
        public List<IMethodAnalyzer> MethodAnalyzers => new List<IMethodAnalyzer>();

        internal DuctTapeRegistry()
        {
            RegisterNamespaceAnalyzers();
            RegisterClassAnalyzers();
            RegisterMethodAnalyzers();
        }

        private void RegisterNamespaceAnalyzers()
        {
            throw new NotImplementedException();
        }

        private void RegisterClassAnalyzers()
        {
            throw new NotImplementedException();
        }

        private void RegisterMethodAnalyzers()
        {
            throw new NotImplementedException();
        }
    }
}
