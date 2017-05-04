using CodeSmeller.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;

namespace CodeSmeller.Analyzers
{
    public class StatementCounter : IAnalyzer<MethodDeclarationSyntax>
    {
        ConcurrentDictionary<int, long> _data;
        dynamic _report;
        string _summary;

        public StatementCounter()
        {
            Initialize();
        }
        
        public void Analyze(MethodDeclarationSyntax syntax, string file)
        {
            if (syntax.Body == null || syntax.Body.Statements.Count == 0) return;

            var walker = new StatementWalker();
            walker.Visit(syntax);
            _data.AddOrUpdate(walker.Count, 1, (k, v) => v + 1);
        }

        public void Initialize()
        {
            _data = new ConcurrentDictionary<int, long>();
        }

        public string Report()
        {
            CompileAnalysis();
            if (_report == null) return null;

            return JsonConvert.SerializeObject(_report, Formatting.Indented);
        }

        public string Summarize()
        {
            CompileAnalysis();
            return _summary;
        }

        private void CompileAnalysis()
        {
            if (_report != null) return;

            CompileReport();
            CreateSummary();
        }

        private void CompileReport()
        {
            long methodCount = _data.Sum(x => x.Value);
            if (methodCount == 0) return;

            _report = new
            {
                analyzer = "Statement Counter",
                stats = new
                {
                    methodCount = methodCount,
                    avgStatementsPerMethod = _data.Sum(x => x.Key * x.Value) / methodCount,
                    highStatementMethods = _data.Where(x => x.Key > 5).Sum(x => x.Value)
                }
            };
        }

        private void CreateSummary()
        {
            if (_report == null) return;

            var stats = _report.stats;
            double highPercent = Math.Round(((double)stats.highStatementMethods / (double)stats.methodCount) * 100d);
            _summary = $"Statement Counter\r\n\tOf {stats.methodCount} methods analyzed: \r\n\t\t{highPercent}% have a large number of statements\r\n\t\tThe average number of statements per method is {stats.avgStatementsPerMethod}";
        }

        private class StatementWalker : CSharpSyntaxWalker
        {
            List<Type> _statementTypes;

            public StatementWalker()
            {
                _statementTypes = new List<Type>
                {
                    typeof(CommonForEachStatementSyntax),
                    typeof(DoStatementSyntax),
                    typeof(ExpressionStatementSyntax),
                    typeof(ForStatementSyntax),
                    typeof(IfStatementSyntax),
                    typeof(LocalDeclarationStatementSyntax),
                    typeof(LocalFunctionStatementSyntax),
                    typeof(SwitchStatementSyntax),
                    typeof(WhileStatementSyntax)
                };
            }

            public int Count { get; private set; }

            public override void Visit(SyntaxNode node)
            {
                if (_statementTypes.Contains(node.GetType()))
                {
                    Count += 1;
                }

                base.Visit(node);
            }
        }
    }
}
