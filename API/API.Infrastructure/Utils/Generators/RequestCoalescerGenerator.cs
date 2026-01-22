using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace API.Infrastructure.Utils.Generators;

[Generator]
public class RequestCoalescerGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not SyntaxReceiver receiver)
            return;

        foreach (var method in receiver.CandidateMethods)
        {
            var model = context.Compilation.GetSemanticModel(method.SyntaxTree);
            var symbol = model.GetDeclaredSymbol(method) as IMethodSymbol;

            if (symbol == null)
                continue;

            var attr = symbol.GetAttributes()
                .FirstOrDefault(a => a.AttributeClass?.Name == "RequestCoalescerAttribute");

            if (attr == null)
                continue;

            var excludeParams = attr.ConstructorArguments.Length > 0
                ? attr.ConstructorArguments[0].Values.Select(v => v.Value?.ToString()!).ToArray()
                : Array.Empty<string>();

            var methodName = symbol.Name;
            var ns = symbol.ContainingNamespace.ToDisplayString();
            var typeName = symbol.ContainingType.Name;
            var keyName = $"{methodName}Key";

            var parameters = symbol.Parameters
                .Where(p => !excludeParams.Contains(p.Name))
                .ToArray();

            var keyProps = string.Join(Environment.NewLine,
                parameters.Select(p =>
                    $"public {p.Type.ToDisplayString()} {p.Name} {{ get; init; }}"));

            var keyCtorParams = string.Join(", ",
                parameters.Select(p => $"{p.Type.ToDisplayString()} {p.Name}"));

            var keyCtorAssignments = string.Join(Environment.NewLine,
                parameters.Select(p => $"{p.Name} = {p.Name};"));

            var keyArgs = string.Join(", ",
                parameters.Select(p => p.Name));

            var returnType = symbol.ReturnType.ToDisplayString();

            var coalescerName = $"{methodName}Coalescer";

            var wrapperParams = string.Join(", ",
                symbol.Parameters.Select(p => $"{p.Type.ToDisplayString()} {p.Name}"));

            var wrapperArgs = string.Join(", ",
                symbol.Parameters.Select(p => p.Name));

            var builder = new StringBuilder($@"
using System;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Utils;

namespace {ns}
{{
    public partial class {typeName}
    {{
        private static readonly RequestCoalescer<{keyName}, {returnType}> {coalescerName}
            = new RequestCoalescer<{keyName}, {returnType}>();

        private record {keyName}
        {{
{keyProps}

            public {keyName}({keyCtorParams})
            {{
{keyCtorAssignments}
            }}
        }}

        public Task<{returnType}> {methodName}_CoalescedAsync({wrapperParams}, CancellationToken ct = default)
        {{
            var key = new {keyName}({keyArgs});
            return {coalescerName}.ExecuteAsync(
                key,
                token => {methodName}({wrapperArgs}, token),
                ct);
        }}
    }}
}}
");

            context.AddSource($"{typeName}_{methodName}_RequestCoalescer.g.cs", builder.ToString());
        }
    }

    private class SyntaxReceiver : ISyntaxReceiver
    {
        public List<MethodDeclarationSyntax> CandidateMethods { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is MethodDeclarationSyntax method &&
                method.AttributeLists.Count > 0)
            {
                CandidateMethods.Add(method);
            }
        }
    }
}
