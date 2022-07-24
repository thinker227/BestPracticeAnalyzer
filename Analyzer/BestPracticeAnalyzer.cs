using System.Linq;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class BestPracticeAnalyzer : DiagnosticAnalyzer {

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
		ImmutableArray.Create(Descriptor);

	private static DiagnosticDescriptor Descriptor { get; } = new(
		"BP0001",
		"Assure best practices",
		"Consider whether this code is adhering to best practices",
		"Quality",
		DiagnosticSeverity.Info,
		true,
		"Best practices are important to consider for every line of code; please consult the documentation as to whether this line is adhering to best practices"
	);



	private void Execute(SyntaxTreeAnalysisContext context) {
		var text = context.Tree.GetText();
		var textLines = text.Lines
			.Where(l => !string.IsNullOrWhiteSpace(l.ToString()));
		foreach (var line in textLines) {
			var diagnostic = Diagnostic.Create(
				Descriptor,
				Location.Create(context.Tree, TrimSpan(line))
			);
			context.ReportDiagnostic(diagnostic);
		}
	}

	private TextSpan TrimSpan(TextLine text) {
		int start;
		for (start = text.Span.Start; start < text.Span.End; start++) {
			if (!char.IsWhiteSpace(text.Text![start])) break;
		}
		int end;
		for (end = text.Span.End; end >= text.Span.Start; end--) {
			if (!char.IsWhiteSpace(text.Text![end-1])) break;
		}
		return TextSpan.FromBounds(start, end);
	}

	public override void Initialize(AnalysisContext context) {
		context.EnableConcurrentExecution();
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

		context.RegisterSyntaxTreeAction(Execute);
	}

}
