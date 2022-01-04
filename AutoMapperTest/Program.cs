using System.Collections.Concurrent;
using System.Diagnostics;
using AutoMapper;
using AutoMapperTest;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace AutoMapperTest
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(new[]
            {
                typeof(BenchmarkTest),
            });

            switcher.Run(args, new Config());

            Console.ReadLine();
        }
    }
}



