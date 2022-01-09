using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using FastExpressionCompiler;
using Mapster;

namespace MapsterTest
{
    public class BenchmarkMapperTest
    {
        readonly UserDO userDO = new();

        //Mapster
        readonly TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;

        //AutoMapper
        readonly Mapper autoMapper = new(new MapperConfiguration(cfg => cfg.CreateMap<UserDO, UserDTO>()));

        public BenchmarkMapperTest()
        {
            //Mapster
            config.Compiler = exp => exp.CompileFast();

            userDO.UserName = "Name";
            userDO.UserAddr = "Addr";
        }

        [Benchmark]
        public void Mapster()
        {
            //UserDTO userDTO = userDO.Adapt<UserDTO>(
            //    config.Fork(forked => forked.ForType<UserDO, UserDTO>().Map("UserName", "UserName")));
            UserDTO userDTO = userDO.Adapt<UserDTO>();
        }

        [Benchmark]
        public void AutoMapper()
        {
            UserDTO userDTO = autoMapper.Map<UserDTO>(userDO);
        }
    }
}
