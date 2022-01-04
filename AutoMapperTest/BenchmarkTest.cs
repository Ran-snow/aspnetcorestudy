using System.Collections.Concurrent;
using System.Diagnostics;
using AutoMapper;
using AutoMapperTest;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace AutoMapperTest
{
    public class BenchmarkTest
    {
        UserDO userDO = new()
        {
            UserAge = 1,
            UserName = "123",
            UserNick = "666"
        };

        Mapper mapper;

        public BenchmarkTest()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDO, UserEntity>());
            mapper = new Mapper(config);
        }

        [Benchmark]
        public void AutoMapperCache()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                GetMapper((typeof(UserDO).ToString() + typeof(UserEntity).ToString()).GetHashCode().ToString(), () =>
                        new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<UserDO, UserEntity>();

                        }).CreateMapper()).Map<UserEntity>(userDO);
            }
        }

        [Benchmark]
        public void AutoMapper()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                mapper.Map<UserEntity>(userDO);
            }
        }


        [Benchmark]
        public void 原生()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                UserEntity user = new()
                {
                    UserName = userDO.UserName,
                    UserAge = userDO.UserAge,
                    UserNick = userDO.UserNick,
                };
            }
        }

        private static readonly ConcurrentDictionary<string, IMapper> cacheDic = new();
        private static IMapper GetMapper(string key, Func<IMapper> func)
        {
            if (cacheDic.TryGetValue(key, out IMapper? mapper))
            {
                return mapper;
            }
            else
            {
                //内存保护
                if (cacheDic.Count > 10000)
                {
                    throw new OverflowException("Capinfo.MUSC.BDP.Core AutoMapperHelper报错：目前Mapper过多，请考虑您是否正常调用此方法");
                }

                return cacheDic.GetOrAdd(key, func());
            }
        }
    }
}
