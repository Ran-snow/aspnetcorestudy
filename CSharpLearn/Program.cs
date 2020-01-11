using System;

namespace CSharpLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            string jkjk = "奥里给";
            //jkjk.AsSpan
            Span<char> span = new Span<char>(jkjk.ToCharArray());

            var s = span.Slice(1, 1);

            Console.WriteLine(s.ToString());

            Console.WriteLine("Hello World!");
        }
    }
}
