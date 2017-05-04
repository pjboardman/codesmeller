using CodeSmeller.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json;

namespace CodeSmeller.Analyzers
{
    public class GuardClauseAnalyzer : IAnalyzer<MethodDeclarationSyntax>
    {
        ConcurrentBag<Tracked> _data;
        dynamic _report;
        string _summary;

        public GuardClauseAnalyzer()
        {
            Initialize();
        }

        public void Initialize()
        {
            _data = new ConcurrentBag<Tracked>();
        }

        public void Analyze(MethodDeclarationSyntax syntax, string file)
        {
            if (syntax.Body == null || syntax.Body.Statements.Count == 0) return;

            var methodStatements = syntax.Body.Statements.ToList();

            if (!HasConditionals(methodStatements)) return;

            var tracked = Track(syntax, file);

            EvaluateConditionals(methodStatements, tracked);

            _data.Add(tracked);
        }

        private bool HasConditionals(List<StatementSyntax> methodStatements)
        {
            return methodStatements != null && methodStatements.OfType<IfStatementSyntax>().Any();
        }

        private void EvaluateConditionals(List<StatementSyntax> methodStatements, Tracked tracked)
        {
            if (IsCandidate(methodStatements))
            {
                tracked.Candidate = true;
            }
            else if (HasGuardClause(methodStatements))
            {
                tracked.HasGuardClause = true;
            }
        }

        private bool IsCandidate(List<StatementSyntax> statements)
        {
            if (statements.Last() is IfStatementSyntax) return true;

            var conditionals = statements.OfType<IfStatementSyntax>();

            if (conditionals.Any(x => x.Else != null)) return true;

            return false;
        }

        private bool HasGuardClause(List<StatementSyntax> statements)
        {
            var firstConditional = statements.OfType<IfStatementSyntax>().First();
            var indexOfFirstConditional = statements.IndexOf(firstConditional);

            return indexOfFirstConditional < statements.Count - 1 
                && firstConditional.Descendants<ReturnStatementSyntax>().Any();
        }

        private Tracked Track(MethodDeclarationSyntax syntax, string file)
        {
            return new Tracked
            {
                Name = syntax.Identifier.Text,
                FileName = file
            };

        }

        public string Report()
        {
            CompileAnalysis();
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
            var analysis = _data
                .Where(tracked => tracked.Candidate)
                .GroupBy(d => d.FileName)
                .Select(group => 
                {
                    return new
                    {
                        file = group.Key,
                        methods = group.Select(g => g.Name).ToArray()
                    };
                });

            _report = new
            {
                analyzer = "Guard Clause",
                stats = new
                {
                    methodsAnalyzed = _data.Count,
                    methodsWithGuardClause = _data.Count(x => x.HasGuardClause),
                    candidates = _data.Count(x => x.Candidate)
                },
                analysis = new 
                {
                    candidates = analysis.ToArray()
                }
            };
        }

        private void CreateSummary()
        {
            var stats = _report.stats;
            double percentWith = Math.Round(((double)stats.methodsWithGuardClause / (double)stats.methodsAnalyzed) * 100d);
            _summary = $"Guard Clause\r\n \tOf {stats.methodsAnalyzed} methods analyzed: \r\n\t\t{percentWith}% had guard clauses \r\n\t\t{stats.candidates} methods could potentially use guard clauses";
        }

        private class Tracked
        {
            internal bool HasGuardClause { get; set; }
            internal bool Candidate { get; set; }
            internal string Name { get; set; }
            internal string FileName { get; set; }
        }
    }
}
