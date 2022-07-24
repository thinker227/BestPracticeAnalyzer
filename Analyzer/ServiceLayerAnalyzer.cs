using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ServiceLayerAnalyzer : DiagnosticAnalyzer {

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
		ImmutableArray.Create(Descriptor);

	private static DiagnosticDescriptor Descriptor { get; } = new(
		"BP0002",
		"Consider adding service layer",
		"Consider adding a service layer for this logic",
		"Quality",
		DiagnosticSeverity.Info,
		true,
		"Service layers provide more maintainable code; consider separating this code into a service layer"
	);



	private void Execute(OperationAnalysisContext context) {
		var operation = (IBlockOperation)context.Operation;
		if (operation.Parent is IMethodBodyBaseOperation) return;
		if (operation.Operations.Length < 2) return;

		var syntax = operation.Syntax;
		var location = Location.Create(syntax.SyntaxTree, syntax.Span);
		var diagnostic = Diagnostic.Create(
			Descriptor,
			location
		);
		context.ReportDiagnostic(diagnostic);
	}

	public override void Initialize(AnalysisContext context) {
		context.EnableConcurrentExecution();
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

		context.RegisterOperationAction(Execute, OperationKind.Block);
	}

}
