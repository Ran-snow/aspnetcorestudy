using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSourceGenerator
{
    public static partial class Program
    {
        static void Main(string[] args)
        {
            // HelloFrom("Generated Code");

            UserDO userDO = new()
            {
                UserAge = 1,
                UserName = "123",
                UserNick = "666"
            };

            Stopwatch stopwatch = Stopwatch.StartNew();
            List<UserEntity> users = new();

            var mapper = new UserDO2Entity();
            for (int i = 0; i < 1_000_000; i++)
            {
                users.Add(mapper.Map(userDO));
            }

            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.WriteLine(users.Count);
            stopwatch = Stopwatch.StartNew();
            users = new();

            for (int i = 0; i < 1_000_000; i++)
            {
                users.Add(MapTest<UserDO, UserEntity>(userDO));
            }

            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.WriteLine(users.Count);

            Console.ReadLine();
        }

        // static partial void HelloFrom(string name);

        static TDest MapTest<TSource, TDest>(TSource source)
            where TDest : class, new()
            where TSource : class
        {
            TDest dest = new TDest();
            Type sourceType = typeof(TSource), destType = typeof(TDest);

            foreach (var destPropertyInfo in destType.GetProperties())
            {
                var sourcePropertyInfo = sourceType.GetProperty(destPropertyInfo.Name);

                if (sourcePropertyInfo != null)
                {
                    if (destPropertyInfo.PropertyType.IsValueType)
                    {
                        destPropertyInfo.SetValue(dest, sourcePropertyInfo.GetValue(source));
                    }
                    else if (destPropertyInfo.PropertyType.IsArray)
                    {
                        destPropertyInfo.SetValue(dest, ((Array)sourcePropertyInfo.GetValue(source)).Clone());
                    }
                    else
                    {
                        //if (sourcePropertyInfo.PropertyType.GetCustomAttributes(true).Any(x => ((Attribute)x)..AttributeType==typeof(AutoMapperLibrary.Attributes.AutomapToAttribute) && x.DestType == destPropertyInfo.PropertyType)
                        //destPropertyInfo.SetValue(dest, MapTest(sourcePropertyInfo.GetValue(source)));

                    }
                }

            }
            return dest;
        }
    }
}