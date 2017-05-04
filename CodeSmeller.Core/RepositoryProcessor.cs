using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSmeller.Core
{
    public class RepositoryProcessor
    {
        private readonly IAnalyzerRegistry _registry;
        private readonly Smeller _smeller;

        public RepositoryProcessor(IAnalyzerRegistry registry) : this(registry, new Smeller(registry))
        {
        }

        public RepositoryProcessor(IAnalyzerRegistry registry, Smeller smeller)
        {
            _registry = registry;
            _smeller = smeller;
        }

        public void Process(string directory)
        {
            Analyze(directory);
            Summarize();
            Report();
        }

        private void Analyze(string directory)
        {
            string[] files = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);

            Parallel.ForEach(files, _smeller.Smell);
        }

        private void Summarize()
        {
            var builder = new StringBuilder();
            builder.AppendLine(Header("Namespace"));
            _registry.NamespaceAnalyzers.ToList().ForEach(x => builder.AppendLine(x.Summarize()));
            builder.AppendLine(Header("Class"));
            _registry.ClassAnalyzers.ToList().ForEach(x => builder.AppendLine(x.Summarize()));
            builder.AppendLine(Header("Method"));
            _registry.MethodAnalyzers.ToList().ForEach(x => builder.AppendLine(x.Summarize()));

            Console.WriteLine(builder.ToString());
        }

        private void Report()
        {
            var builder = new StringBuilder();
            builder.AppendLine("{ analysis: {");

            builder.AppendLine(ReportHeader("namespaces"));
            _registry.NamespaceAnalyzers.ToList().ForEach(x => builder.AppendLine(x.Report()));
            builder.AppendLine("],");

            builder.AppendLine(ReportHeader("classes"));
            _registry.ClassAnalyzers.ToList().ForEach(x => builder.AppendLine(x.Report()));
            builder.AppendLine("],");

            builder.AppendLine(ReportHeader("methods"));
            _registry.MethodAnalyzers.ToList().ForEach(x => builder.AppendLine(x.Report()));
            builder.AppendLine("]");

            builder.AppendLine("}}");
            File.WriteAllText("SmellReport.json", builder.ToString());
        }

        private static string ReportHeader(string type)
        {
            return $"{type}: [";

        }

        private string Header(string type)
        {
            string separator = new string('=', 60);

            return $"{separator}\r\n{type} Analysis:\r\n{separator}";
        }
    }
}
