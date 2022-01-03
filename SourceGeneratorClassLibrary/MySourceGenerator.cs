using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace SourceGeneratorClassLibrary
{
    //[Generator]
    public class MySourceGenerator //: ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);
            string source = $@"
using System;

namespace {mainMethod.ContainingNamespace.ToDisplayString()}
{{
    public static partial class {mainMethod.ContainingType.Name}
    {{
        static partial void HelloFrom(string name)
        {{
            Console.WriteLine($""Generator says: Hi from '{{name}}'"");
        }}
    }}
}}
";
            context.AddSource("generatedSource",source);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
