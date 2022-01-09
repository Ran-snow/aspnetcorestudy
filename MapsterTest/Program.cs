using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using FastExpressionCompiler;
using System.Data;
using System.Data.SqlClient;
using BenchmarkDotNet.Running;

namespace MapsterTest
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            BenchmarkDataReaderTest benchmarkTest = new();
            await benchmarkTest.SchemaTableDataReader();
            Console.ReadKey();

            var switcher = new BenchmarkSwitcher(new[]
            {
                typeof(BenchmarkMapperTest),
                typeof(BenchmarkDataReaderTest),
            });

            switcher.Run(args, new Config());

            Console.ReadKey();
        }
    }
}