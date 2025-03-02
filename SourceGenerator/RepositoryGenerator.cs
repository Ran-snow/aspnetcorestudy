using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SGRepository
{
    [Generator]
    public partial class RepositoryGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // 1. 过滤所有继承 IRepository 的接口
            var repositoryInterfaces = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is InterfaceDeclarationSyntax ids
                        && ids.BaseList?.Types.Any(t => t.Type.ToString() == "IRepository") == true,
                    transform: (ctx, _) => (InterfaceDeclarationSyntax)ctx.Node
                )
                .Collect();

            // 2. 结合编译符号生成代码
            context.RegisterSourceOutput(context.CompilationProvider.Combine(repositoryInterfaces),
                (spc, source) => GenerateCode(spc, source.Left, source.Right));
        }

        private void GenerateCode(
            SourceProductionContext context,
            Compilation compilation,
            ImmutableArray<InterfaceDeclarationSyntax> interfaces)
        {
            //Debugger.Launch();

            var code = new StringBuilder();
            code.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            code.AppendLine("using WebApplicationUnitTestDemo.Repositories;");
            code.AppendLine("using WebApplicationUnitTestDemo.Repositories.impl;");
            code.AppendLine("namespace WebApplicationUnitTestDemo {");
            code.AppendLine("   public static class RepositoryExtensions {");
            code.AppendLine("       public static IServiceCollection AddRepositories(this IServiceCollection services) {");

            foreach (var interfaceSyntax in interfaces)
            {
                var interfaceName = interfaceSyntax.Identifier.Text;
                var implementationName = $"{interfaceName.Substring(1)}";

                code.AppendLine($"            services.AddScoped<{interfaceName}, {implementationName}>();");
            }

            code.AppendLine("            return services;");
            code.AppendLine("      }");
            code.AppendLine("   }");
            code.AppendLine("}");

            context.AddSource("Repositories.g.cs", SourceText.From(code.ToString(), Encoding.UTF8));
        }
    }
}