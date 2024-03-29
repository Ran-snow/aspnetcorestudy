﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppT4
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Must .net fx !!!
            string assemblyFile = @"\net48\ObjsClassLibrary.dll";
            string interfaceName = "IMap";

            Assembly assembly = Assembly.LoadFrom(assemblyFile);
            if (assembly == default) return;
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //Console.WriteLine("AssemblyName");
            //Console.WriteLine(assembly.GetName().ToString());
            //Console.WriteLine(string.Empty);

            foreach (var sourceType in assembly.GetTypes())
            {
                var interfaceAssigned = GetDestType_InterfaceStrategy(sourceType, interfaceName);
                if (interfaceAssigned != default)
                {
                    var destType = interfaceAssigned.GenericTypeArguments[0];

                    //Console.WriteLine("NamespaceName");
                    //Console.WriteLine(sourceType.Namespace);

                    //Console.WriteLine("TypeName");
                    //Console.WriteLine(sourceType.Name);

                    List<MemberInfo>? sourceMembers = sourceType.GetMembers()?.ToList().Where(x => x.MemberType == MemberTypes.Property).ToList();
                    if (sourceMembers == default) continue;
                    List<MemberInfo>? destMembers = destType.GetMembers()?.ToList().Where(x => x.MemberType == MemberTypes.Property).ToList();
                    if (destMembers == default) continue;

                    var props = sourceMembers.Intersect(destMembers, new CompareMember());
                    if (!props.Any()) continue;

                    //Console.WriteLine("=================");
                    //manager.StartNewFile($"{destType.Name}.Generated.cs");
                    WriteLine($"using {destType.Namespace};");
                    WriteLine(string.Empty);
                    WriteLine($"namespace {sourceType.Namespace}");
                    WriteLine("{");
                    WriteLine($"    {GetAttr(sourceType.Attributes)} static class ClassTest");
                    WriteLine("    {");
                    WriteLine($"        {GetAttr(sourceType.Attributes)} static {destType.Name} MapTo{destType.Name}(this {sourceType.Name} obj)");
                    WriteLine("        {");
                    WriteLine($"            {destType.Name} dest = new {destType.Name}();");
                    WriteLine(string.Empty);
                    foreach (var item in props)
                    {
                        WriteLine($"            dest.{item.Name} = obj.{item.Name};");
                    }
                    WriteLine(string.Empty);
                    WriteLine($"            return dest;");
                    WriteLine("        }");
                    WriteLine("    }");
                    WriteLine("}");
                    //manager.EndBlock();
                    //manager.Process(true);

                    //Console.WriteLine(string.Empty);
                }
            }
        }

        private static void WriteLine(string text) => Console.WriteLine(text);

        private static string GetAttr(TypeAttributes typeAttributes) => typeAttributes == TypeAttributes.Public ? "public" : "internal";

        private static Type? GetDestType_InterfaceStrategy(Type sourceType, string interfaceName)
        {
            return sourceType.GetInterface(interfaceName + "`1");
        }

        internal class CompareMember : IEqualityComparer<MemberInfo>
        {
            public bool Equals(MemberInfo? x, MemberInfo? y) => x?.Name == y?.Name && x?.GetType() == y?.GetType();

            public int GetHashCode([DisallowNull] MemberInfo obj) => obj.Name.GetHashCode();
        }
    }
}