using System;
using System.Collections;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HashTableTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Hashtable hashtable = Hashtable.Synchronized(new Hashtable());
            Hashtable hashtable = new Hashtable();
            //hashtable["a"] = 0;

            _ = Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine(hashtable["a"]);
                }
            });

            await Task.Delay(10);

            Parallel.ForEach(Enumerable.Range(5, 1000), a =>
            {
                var found = hashtable["a"];
                if (found == null)
                {
                    lock (hashtable)
                    {
                        found = hashtable["a"];
                        if (found == null)
                        {
                            hashtable["a"] = a;
                            Console.WriteLine("Parallel->" + a);
                        }
                    }
                }
            });

            //var res = Enumerable.Range(0, 100).Select(i => i.ToString()).AsParallel().Select((v, i) =>
            //{

            //    var found = hashtable["a"];
            //    if (found == null)
            //    {
            //        hashtable["a"] = v;
            //    }

            //    return i;

            //}).ToList();


            //lock (hashtable)
            //{
            //    int i = 0;
            //    while (i < 10)
            //    {
            //        hashtable["a"] = new Random().Next();
            //        Thread.Sleep(100);
            //        i = ++i;
            //    }
            //}

            await Task.Delay(100);

            Console.WriteLine("OK");
            //Console.ReadKey();
        }
    }
}
